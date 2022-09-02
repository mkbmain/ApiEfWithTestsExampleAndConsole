using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.IoC;
using ServiceLayer.Service.Customer;
using ServiceLayer.Service.Orders;
using Shouldly;
using Xunit;

namespace ServiceLayer.Tests.IoC;

public class TestIoC
{
    // I have never found a good way to do this 
    
    
    
    /// <summary>
    ///  this way works but ensuring when people add new things they update it proves challenging
    /// </summary>
    [Fact]
    public void EnsureWeAreRegisteredOption1()
    {
        var items = GetDi();

        var service = items.GetService<ICustomerService>();
        var service2 = items.GetService<IOrderService>();

        service.ShouldNotBeNull();
        service.ShouldBeOfType<CustomerService>();
        service2.ShouldNotBeNull();
        service2.ShouldBeOfType<OrderService>();
    }
    
    /// <summary>
    /// this now checks every interface with in the service layer is registerd and can be got.
    /// so no need to update but its flakey and liable to false postives what if you also have
    /// concrete classes with out interfaces or interfaces with multiple implementations
    ///  does not ensure the correct thing is registered
    /// </summary>
    [Fact]
    public void EnsureWeAreRegisteredDynamicallyOption2()
    {
        var items = GetDi();

        var getAllSerices = Assembly.GetAssembly(typeof(IOrderService))
            .GetTypes().Where(t => t.IsInterface && t.IsAbstract == false);

        foreach (var type in getAllSerices)
        {
            items.GetService(type).ShouldNotBeNull();
        }
    }

    private static IServiceProvider GetDi()
    {
        IConfiguration configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var serviceCollection = new ServiceCollection();
        ServiceLayerIoC.Add(serviceCollection, configuration);

        return serviceCollection.BuildServiceProvider();
    }
}