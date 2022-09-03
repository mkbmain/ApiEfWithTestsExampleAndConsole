using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ServiceLayer.Models;
using Xunit;

namespace SimpleEfApi.Tests.ControllersTests.CustomerControllerTests
{
    public class GetUserByIdTests
    {
        private const int Id = 52;
        [Fact]
        public async Task Ensure_on_success_we_report()
        {
            var (sut, mockCustomerService) = CustomerControllerFactory.Build();
            var expectedResponse = new ServiceResponse<CustomerResponse>() { Status = ServiceStatus.Success, Data = new CustomerResponse{Email = "test@gmail.com", Name = "mike"} };
            mockCustomerService.Setup(e => e.GetCustomerById(Id))
                .ReturnsAsync(expectedResponse);


            var result = await sut.GetUserById(Id);

            var response = result as ObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            var data = response.Value as ServiceResponse<CustomerResponse>;
            Assert.Equal(expectedResponse, data);
        }

        [Fact]
        public async Task Ensure_on_bad_request_we_report()
        {
            var (sut, mockCustomerService) = CustomerControllerFactory.Build();
            var expectedResponse = new ServiceResponse<CustomerResponse>() { Status = ServiceStatus.BadRequest,Message = "badMessage"};
            mockCustomerService.Setup(e => e.GetCustomerById(Id))
                .ReturnsAsync(expectedResponse);


            var result = await sut.GetUserById(Id);

            var response = result as ObjectResult;
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            var data = response.Value as ServiceResponse;
            Assert.Equal(expectedResponse.Message, data.Message);
            Assert.Equal(expectedResponse.Status, data.Status);
        }

        [Fact]
        public async Task Ensure_on_error_we_report()
        {
            var (sut, mockCustomerService) = CustomerControllerFactory.Build();
            var expectedResponse = new ServiceResponse<CustomerResponse>() { Status = ServiceStatus.Error, Message = "bad Message should not come back" };
            mockCustomerService.Setup(e => e.GetCustomerById(Id))
                .ReturnsAsync(expectedResponse);


            var result = await sut.GetUserById(Id);

            var response = result as ObjectResult;
            Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
            var data = response.Value as ServiceResponse;
            Assert.Empty(data.Message);
            Assert.Equal(expectedResponse.Status, data.Status);
        }
    }
}