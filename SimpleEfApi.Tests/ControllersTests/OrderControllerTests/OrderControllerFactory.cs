using Moq;
using ServiceLayer.Service.Customer;
using ServiceLayer.Service.Orders;
using SimpleEfApi.Controllers;

namespace SimpleEfApi.Tests.ControllersTests.OrderControllerTests
{
    internal static class OrderControllerFactory
    {
        public static (OrderController Sut, Mock<IOrderService> mockOrderService) Build()
        {
            var orderService = new Mock<IOrderService>();
            return (new OrderController(orderService.Object), orderService);
        }
    }
}
