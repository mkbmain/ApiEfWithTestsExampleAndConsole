using DataLayer;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceLayer.Service.Customer;
using ServiceLayer.Service.Orders;
using SimpleRepo.Repo;

namespace ServiceLayer.Tests.ServiceUnitTests.OrderServiceTests;

public class OrderServiceFactory
{
    public static (OrderService, Mock<ILogger<OrderService>>, Mock<IRepo<ExampleDbContext>>) Generate()
    {
        var mock = new Mock<IRepo<ExampleDbContext>>();
        var logger = new Mock<ILogger<OrderService>>();
        return (new OrderService(mock.Object, logger.Object), logger, mock);
    }
}