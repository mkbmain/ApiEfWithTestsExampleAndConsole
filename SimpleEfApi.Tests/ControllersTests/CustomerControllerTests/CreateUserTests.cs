using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ServiceLayer.Models;
using Xunit;

namespace SimpleEfApi.Tests.ControllersTests.CustomerControllerTests
{
    public class CreateUserTests
    {
        [Fact]
        public async Task Ensure_we_can_add_a_customer()
        {
            const int id = 423;
            var request = CreateCustomerRequest();
            var (sut, mockCustomerService) = CustomerControllerFactory.Build();
            var expectedResponse = new ServiceResponse<int>() { Status = ServiceStatus.Success, Data = id };
            mockCustomerService.Setup(e => e.AddCustomer(request))
                .ReturnsAsync(expectedResponse);

            var result = await sut.CreateUser(request);

            var response = result as ObjectResult;
            Assert.Equal((int)HttpStatusCode.Created, response.StatusCode);
            var data = response.Value as ServiceResponse<int>;
            Assert.Equal(expectedResponse, data);
        }

        [Fact]
        public async Task Ensure_if_error_we_report()
        {
            var request = CreateCustomerRequest();
            var (sut, mockCustomerService) = CustomerControllerFactory.Build();
            var expectedResponse = new ServiceResponse<int>() { Status = ServiceStatus.Error, Message = "should not make it" };
            mockCustomerService.Setup(e => e.AddCustomer(request))
                .ReturnsAsync(expectedResponse);

            var result = await sut.CreateUser(request);

            var response = result as ObjectResult;
            Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
            var data = response.Value as ServiceResponse;
            Assert.Equal("", data.Message);
            Assert.Equal(ServiceStatus.Error, data.Status);
        }


        [Fact]
        public async Task Ensure_bad_request_we_report()
        {
            var request = CreateCustomerRequest();
            var (sut, mockCustomerService) = CustomerControllerFactory.Build();
            var expectedResponse = new ServiceResponse<int>() { Status = ServiceStatus.BadRequest, Message = "should GetThis" };
            mockCustomerService.Setup(e => e.AddCustomer(request))
                .ReturnsAsync(expectedResponse);

            var result = await sut.CreateUser(request);

            var response = result as ObjectResult;
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            var data = response.Value as ServiceResponse;
            Assert.Equal(expectedResponse.Message, data.Message);
            Assert.Equal(ServiceStatus.BadRequest, data.Status);
        }

        private static CreateCustomerRequest CreateCustomerRequest() => new CreateCustomerRequest
        {
            BuildingName = "test"
        };
    }
}