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

public class GetCustomerByIdTests
{

    [Fact]
    public async Task Ensure_we_can_GetCustomerById()
    {
        const int id =52;
        var goodCustomer = new Customer
        {
            Id = id,
            Email = "Email",
            DateOfBirth = DateTime.Now.Date.AddDays(-2662),
            Name = "mike",
        };
        var (sut, mocklogger, mockrepo) = CustomerServiceFactory.Generate();
        var list = new List<Customer>()
            {goodCustomer, new Customer {Id = id-2,Email = "whatEver", DateOfBirth = DateTime.Now.Date, Name = "john"}};
        mockrepo.Setup(t =>
                t.Get(It.IsAny<Expression<Func<Customer, bool>>>(),
                    It.IsAny<Expression<Func<Customer, CustomerResponse>>>()))
            .ReturnsAsync((Expression<Func<Customer, bool>> func,
                    Expression<Func<Customer, CustomerResponse>> pro) =>
                list.Where(func.Compile()).Select(pro.Compile()).FirstOrDefault());

        var response = await sut.GetCustomerById(id);

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
        const int id =52;

        var (sut, mocklogger, mockrepo) = CustomerServiceFactory.Generate();

        var exception = new Exception(Guid.NewGuid().ToString());
        mockrepo.Setup(t =>
                t.Get(It.IsAny<Expression<Func<Customer, bool>>>(),
                    It.IsAny<Expression<Func<Customer, CustomerResponse>>>()))
            .ThrowsAsync(exception);

        var response = await sut.GetCustomerById(id);

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
        const int id =52;
        var goodCustomer = new Customer
        {
            Id = id-2,
            Email = "not this one",
            DateOfBirth = DateTime.Now.Date.AddDays(-2662),
            Name = "mike",
        };
        var (sut, mocklogger, mockrepo) = CustomerServiceFactory.Generate();
        var list = new List<Customer>()
            {goodCustomer, new Customer {Email = "Email" + "test", DateOfBirth = DateTime.Now.Date, Name = "john"}};
        mockrepo.Setup(t =>
                t.Get(It.IsAny<Expression<Func<Customer, bool>>>(),
                    It.IsAny<Expression<Func<Customer, CustomerResponse>>>()))
            .ReturnsAsync((Expression<Func<Customer, bool>> func,
                    Expression<Func<Customer, CustomerResponse>> pro) =>
                list.Where(func.Compile()).Select(pro.Compile()).FirstOrDefault());

        var response = await sut.GetCustomerById(id);

        response.Status.ShouldBe(ServiceStatus.BadRequest);
        response.Data.ShouldBeNull();
    }
}