using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.IoC;
using ServiceLayer.Models;
using ServiceLayer.Service.Customer;
using ServiceLayer.Service.Orders;

namespace ConsoleAppExample
{
    /// <summary>
    /// just a example of DI really. this is not tests as its just a example (a test in its own right)
    /// </summary>
    class Program
    {
        private static IServiceProvider GetDi()
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var serviceCollection = new ServiceCollection();
            ServiceLayerIoC.Add(serviceCollection, configuration);

            return serviceCollection.BuildServiceProvider();
        }

        static async Task Main(string[] args)
        {
            var provider = GetDi();

            var ee = provider.GetService<CustomerOrdersDbContext>();
            await ee.Database.MigrateAsync();
            var customerService = provider.GetService<ICustomerService>();
            var customerEmail = $"{Guid.NewGuid():N}@gmail.com";
            var customerResponse = await customerService.AddCustomer(new CreateCustomerRequest
            {
                Name = "mike",
                Email = customerEmail,
                Dob = DateTime.Now.AddYears(-28).AddMonths(-5).AddDays(-2).Date,
                BuildingName = "bigHut",
                BuildingNumber = "15a",
                City = "London",
                PostCode = "0001515",
                Street = "big road",
            });
            var orderService = provider.GetService<IOrderService>();
            var orderId = await orderService.AddOrder(new CreateOrderRequest
            {
                CustomerEmail = customerEmail,
                Total = 5000
            });

            var ordersByEmail = await orderService.GetOrdersForCustomer(customerEmail);
            var ordersById = await orderService.GetOrdersForCustomer(customerResponse.Data);


            var getCustomerById = await customerService.GetCustomerById(customerResponse.Data);
            var getCustomerByEmail = await customerService.GetCustomerByEmail(customerEmail);

            Console.WriteLine("Hello World!");
        }
    }
}