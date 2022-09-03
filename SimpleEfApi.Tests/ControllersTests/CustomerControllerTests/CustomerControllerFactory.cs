using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ServiceLayer.Service.Customer;
using SimpleEfApi.Controllers;

namespace SimpleEfApi.Tests.ControllersTests.CustomerControllerTests
{
    internal static class CustomerControllerFactory
    {
        public static (CustomerController Sut, Mock<ICustomerService> mockCustomerService) Build()
        {
            var mockCustomerService = new Mock<ICustomerService>();
            return (new CustomerController(mockCustomerService.Object), mockCustomerService);
        }
    }
}
