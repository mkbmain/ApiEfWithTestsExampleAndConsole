using DataLayer;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceLayer.Service.Customer;
using SimpleRepo.Repo;

namespace ServiceLayer.Tests.ServiceUnitTests.CustomerServiceTests;

public class CustomerServiceFactory
{
    public static (CustomerService, Mock<ILogger<CustomerService>> logger, Mock<IRepo<ExampleDbContext>>) Generate()
    {
        var mock = new Mock<IRepo<ExampleDbContext>>();
        var logger = new Mock<ILogger<CustomerService>>();
        return (new CustomerService(mock.Object, logger.Object), logger, mock);
    }
}