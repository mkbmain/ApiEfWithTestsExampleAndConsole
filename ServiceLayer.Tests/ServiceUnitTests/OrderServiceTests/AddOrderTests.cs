using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceLayer.Models;
using Shouldly;
using Xunit;

namespace ServiceLayer.Tests.ServiceUnitTests.OrderServiceTests;

public class AddOrderTests
{
    [Fact]
    public async Task Ensure_if_Customer_is_not_found_we_throw()
    {
        var (sut, logger, mockrepo) = OrderServiceFactory.Generate();
        var result = await sut.AddOrder(new CreateOrderRequest {CustomerEmail = "bad@ewt.com"});
        result.Status.ShouldBe(ServiceStatus.BadRequest);
        result.Message.ShouldBe("customer not found");
    }

    [Fact]
    public async Task Ensure_we_can_add_order()
    {
        const string Email = "test";
        const int customerId = 542;
        var (sut, logger, mockrepo) = OrderServiceFactory.Generate();
        mockrepo.Setup(t =>
                t.Get(It.IsAny<Expression<Func<Customer, bool>>>(),
                    It.IsAny<Expression<Func<Customer, Customer>>>()))
            .ReturnsAsync((Expression<Func<Customer, bool>> func,
                    Expression<Func<Customer, Customer>> pro) =>
                new[] {new Customer {Email = Email, Id = customerId}}.Where(func.Compile()).Select(pro.Compile())
                    .FirstOrDefault());

        var result = await sut.AddOrder(new CreateOrderRequest {CustomerEmail = Email, Total = 100});

        result.Status.ShouldBe(ServiceStatus.Success);
        mockrepo.Verify(t => t.Add(It.Is<Order>(x => x.CustomerId == customerId &&
                                                     x.Id == result.Data &&
                                                     x.Created > DateTime.Now.AddSeconds(-2) &&
                                                     x.Created < DateTime.Now.AddDays(2)
                                                     && x.Total == 100)), Times.Once);
    }
    
    [Fact]
    public async Task Ensure_on_error_we_report()
    {
        const string Email = "test";
        var exception = new Exception(Guid.NewGuid().ToString());
        var (sut, logger, mockrepo) = OrderServiceFactory.Generate();
        mockrepo.Setup(t =>
                t.Get(It.IsAny<Expression<Func<Customer, bool>>>(),
                    It.IsAny<Expression<Func<Customer, Customer>>>()))
            .ThrowsAsync(exception);

        var result = await sut.AddOrder(new CreateOrderRequest {CustomerEmail = Email, Total = 100});

        result.Status.ShouldBe(ServiceStatus.Error);
        result.Message.ShouldBe(exception.Message);
        logger.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), 
                It.IsAny<It.IsAnyType>(), exception, 
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), 
            Times.Once);

    }
}