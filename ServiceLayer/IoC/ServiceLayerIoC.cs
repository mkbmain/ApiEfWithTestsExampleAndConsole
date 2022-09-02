using DataLayer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.Service.Customer;
using ServiceLayer.Service.Orders;
using SimpleRepo.Repo;

namespace ServiceLayer.IoC;

public static class ServiceLayerIoC
{
    public static void Add(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddLogging();
        DataLayer.IoC.DataLayerIoC.Add(serviceCollection, configuration);
        serviceCollection.AddScoped<IRepo<ExampleDbContext>, Repo<ExampleDbContext>>();
        serviceCollection.AddScoped<ICustomerService, CustomerService>();
        serviceCollection.AddScoped<IOrderService, OrderService>();
    }
}