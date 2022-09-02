using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceLayer.Models;
using ServiceLayer.Service.Orders;
using Shouldly;
using SimpleRepo.Repo;
using Xunit;

namespace ServiceLayer.Tests.ServiceUnitTests.OrderServiceTests;

public class GetOrdersForCustomerTests
{
    private const string Email = "test@gmail.com";
    private const int CustomerId = 5324;

    [Fact]
    public async Task Ensure_we_can_Get_customer_by_email()
    {
        var (sut, logger, mockrepo) = OrderServiceFactory.Generate();
        await SetupRepo(() => sut.GetOrdersForCustomer(Email), mockrepo, GoodAsserts);
    }

    [Fact]
    public async Task Ensure_we_can_Get_customer_by_id()
    {
        var (sut, logger, mockrepo) = OrderServiceFactory.Generate();
        await SetupRepo(() => sut.GetOrdersForCustomer(CustomerId), mockrepo, GoodAsserts);
    }


    [Fact]
    public async Task Ensure_if_not_found_we_report_by_id()
    {
        var (sut, logger, mockrepo) = OrderServiceFactory.Generate();
        await SetupRepo(() => sut.GetOrdersForCustomer(CustomerId - 15), mockrepo,
            NotFoundAsserts);
    }

    [Fact]
    public async Task Ensure_if_not_found_we_report()
    {
        var (sut, logger, mockrepo) = OrderServiceFactory.Generate();
        await SetupRepo(() => sut.GetOrdersForCustomer(Email + "testy"), mockrepo,
            NotFoundAsserts);
    }

    [Fact]
    public async Task Ensure_if_error_we_log_Id()
    {
        var (sut, logger, mockrepo) = OrderServiceFactory.Generate();
        var exception = new Exception("test");
        mockrepo.Setup(t =>
                t.Get(It.IsAny<Expression<Func<Customer, bool>>>(),
                    It.IsAny<Expression<Func<Customer, OrderResponse>>>()))
            .ThrowsAsync(exception);
        var response = await sut.GetOrdersForCustomer(CustomerId);
        AssertError(response, exception, logger);
    }

    [Fact]
    public async Task Ensure_if_error_we_log_Email()
    {
        var (sut, logger, mockrepo) = OrderServiceFactory.Generate();
        var exception = new Exception("test");
        mockrepo.Setup(t =>
                t.Get(It.IsAny<Expression<Func<Customer, bool>>>(),
                    It.IsAny<Expression<Func<Customer, OrderResponse>>>()))
            .ThrowsAsync(exception);
        var response = await sut.GetOrdersForCustomer(Email);
        AssertError(response, exception, logger);
    }

    private static async Task SetupRepo(Func<Task<ServiceResponse<OrderResponse>>> func,
        Mock<IRepo<ExampleDbContext>> mockrepo, Action<ServiceResponse<OrderResponse>, Customer> asserts)
    {
        var goodCustomer = new Customer()
        {
            Id = CustomerId,
            Email = Email,
            Orders = new List<Order>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.Now,
                    CustomerId = CustomerId,
                    Total = 5263
                }
            }
        };

        var list = new List<Customer>()
        {
            goodCustomer,
            new()
            {
                Id = 512,
                Email = "g532w@gwe.com",
                Orders = new List<Order>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Created = DateTime.Now.AddDays(-1),
                        CustomerId = 512,
                        Total = 510
                    }
                }
            }
        };
        mockrepo.Setup(t =>
                t.Get(It.IsAny<Expression<Func<Customer, bool>>>(),
                    It.IsAny<Expression<Func<Customer, OrderResponse>>>()))
            .ReturnsAsync((Expression<Func<Customer, bool>> func,
                    Expression<Func<Customer, OrderResponse>> pro) =>
                list.Where(func.Compile()).Select(pro.Compile()).FirstOrDefault());

        var response = await func.Invoke();
        asserts.Invoke(response, goodCustomer);
    }

    private static void AssertError(ServiceResponse<OrderResponse> response, Exception exception,
        Mock<ILogger<OrderService>> mocklogger)
    {
        response.Message.ShouldBe(exception.Message);
        response.Status.ShouldBe(ServiceStatus.Error);
        response.Data.ShouldBeNull();
        mocklogger.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(), exception,
                (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
            Times.Once);
    }

    private static void NotFoundAsserts(ServiceResponse<OrderResponse> response, Customer goodCustomer)
    {
        response.Message.ShouldBe("No Item found");
        response.Status.ShouldBe(ServiceStatus.BadRequest);
        response.Data.ShouldBeNull();
    }

    private static void GoodAsserts(ServiceResponse<OrderResponse> response, Customer goodCustomer)
    {
        response.Status.ShouldBe(ServiceStatus.Success);
        var ordersForCustomer = response.Data;
        ordersForCustomer.CustomerId.ShouldBe(goodCustomer.Id);
        ordersForCustomer.Orders.Count().ShouldBe(1);
        ordersForCustomer.Orders.First().Total.ShouldBe(goodCustomer.Orders.First().Total);
        ordersForCustomer.Orders.First().CreatedAt.ShouldBe(goodCustomer.Orders.First().Created);
        ordersForCustomer.Orders.First().OrderId.ShouldBe(goodCustomer.Orders.First().Id);
    }
}