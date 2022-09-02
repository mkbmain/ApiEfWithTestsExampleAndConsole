using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceLayer.Models;
using Shouldly;
using Xunit;

namespace ServiceLayer.Tests.ServiceUnitTests.CustomerServiceTests;

public class GetCustomerByEmailTests
{
    [Fact]
    public async Task Ensure_we_can_GetCustomerByEmail()
    {
        const string Email = "mike@gmail.com";
        var goodCustomer = new Customer
        {
            Id = 542,
            Email = Email,
            DateOfBirth = DateTime.Now.Date.AddDays(-2662),
            Name = "mike",
        };
        var (sut, mocklogger, mockrepo) = CustomerServiceFactory.Generate();
        var list = new List<Customer>()
            {goodCustomer, new Customer {Email = Email + "test", DateOfBirth = DateTime.Now.Date, Name = "john"}};
        mockrepo.Setup(t =>
                t.Get(It.IsAny<Expression<Func<Customer, bool>>>(),
                    It.IsAny<Expression<Func<Customer, CustomerResponse>>>()))
            .ReturnsAsync((Expression<Func<Customer, bool>> func,
                    Expression<Func<Customer, CustomerResponse>> pro) =>
                list.Where(func.Compile()).Select(pro.Compile()).FirstOrDefault());

        var response = await sut.GetCustomerByEmail(Email);

        response.Status.ShouldBe(ServiceStatus.Success);
        var customer = response.Data;
        customer.Email.ShouldBe(goodCustomer.Email);
        customer.Name.ShouldBe(goodCustomer.Name);
        customer.Id.ShouldBe(goodCustomer.Id);
        customer.Dob.ShouldBe(goodCustomer.DateOfBirth);
    }
    
    [Fact]
    public async Task Ensure_if_we_error_we_log()
    {
        const string Email = "mike@gmail.com";

        var (sut, mocklogger, mockrepo) = CustomerServiceFactory.Generate();

        var exception = new Exception(Guid.NewGuid().ToString());
        mockrepo.Setup(t =>
                t.Get(It.IsAny<Expression<Func<Customer, bool>>>(),
                    It.IsAny<Expression<Func<Customer, CustomerResponse>>>()))
            .ThrowsAsync(exception);

        var response = await sut.GetCustomerByEmail(Email);

        response.Status.ShouldBe(ServiceStatus.Error);
        response.Message.ShouldBe(exception.Message);
        response.Data.ShouldBeNull();
        mocklogger.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), 
                It.IsAny<It.IsAnyType>(), exception, 
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), 
            Times.Once);

    }

    [Fact]
    public async Task Ensure_if_not_found_we_reports()
    {
        const string Email = "mike@gmail.com";
        var goodCustomer = new Customer
        {
            Id = 542,
            Email = Email + "not this one",
            DateOfBirth = DateTime.Now.Date.AddDays(-2662),
            Name = "mike",
        };
        var (sut, mocklogger, mockrepo) = CustomerServiceFactory.Generate();
        var list = new List<Customer>()
            {goodCustomer, new Customer {Email = Email + "test", DateOfBirth = DateTime.Now.Date, Name = "john"}};
        mockrepo.Setup(t =>
                t.Get(It.IsAny<Expression<Func<Customer, bool>>>(),
                    It.IsAny<Expression<Func<Customer, CustomerResponse>>>()))
            .ReturnsAsync((Expression<Func<Customer, bool>> func,
                    Expression<Func<Customer, CustomerResponse>> pro) =>
                list.Where(func.Compile()).Select(pro.Compile()).FirstOrDefault());

        var response = await sut.GetCustomerByEmail(Email);

        response.Status.ShouldBe(ServiceStatus.BadRequest);
        response.Data.ShouldBeNull();
    }
}