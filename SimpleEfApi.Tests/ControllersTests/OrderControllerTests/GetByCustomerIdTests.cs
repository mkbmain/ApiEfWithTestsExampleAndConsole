using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ServiceLayer.Models;
using Xunit;

namespace SimpleEfApi.Tests.ControllersTests.OrderControllerTests;

public class GetByCustomerIdTests
{
    [Fact]
    public async Task Ensure_on_success_we_report()
    {
        const int customerId = 42;
        var (sut, mockOrderService) = OrderControllerFactory.Build();
        var expectedResponse = new ServiceResponse<OrderResponse>()
        {
            Status = ServiceStatus.Success, Data = new OrderResponse
            {
                CustomerId = customerId,
            }
        };
        mockOrderService.Setup(e => e.GetOrdersForCustomer(customerId))
            .ReturnsAsync(expectedResponse);

        var result = await sut.GetByCustomerId(customerId);
        var response = result as ObjectResult;
        Assert.Equal((int) HttpStatusCode.OK, response.StatusCode);
        var data = response.Value as ServiceResponse<OrderResponse>;
        Assert.Equal(expectedResponse, data);
    }

    [Fact]
    public async Task Ensure_on_error_we_report()
    {
        const int customerId = 42;
        var (sut, mockOrderService) = OrderControllerFactory.Build();
        var expectedResponse = new ServiceResponse<OrderResponse>() {Status = ServiceStatus.Error, Message = "test"};
        mockOrderService.Setup(e => e.GetOrdersForCustomer(customerId))
            .ReturnsAsync(expectedResponse);

        var result = await sut.GetByCustomerId(customerId);
        var response = result as ObjectResult;
        Assert.Equal((int) HttpStatusCode.InternalServerError, response.StatusCode);
        var data = response.Value as ServiceResponse;
        Assert.Equal(expectedResponse.Status, data.Status);
        Assert.Empty(data.Message);
    }
    
    [Fact]
    public async Task Ensure_on_badrequest_we_report()
    {
        const int customerId = 42;
        var (sut, mockOrderService) = OrderControllerFactory.Build();
        var expectedResponse = new ServiceResponse<OrderResponse>() {Status = ServiceStatus.BadRequest, Message = "test"};
        mockOrderService.Setup(e => e.GetOrdersForCustomer(customerId))
            .ReturnsAsync(expectedResponse);

        var result = await sut.GetByCustomerId(customerId);
        var response = result as ObjectResult;
        Assert.Equal((int) HttpStatusCode.BadRequest, response.StatusCode);
        var data = response.Value as ServiceResponse;
        Assert.Equal(expectedResponse.Status, data.Status);
        Assert.Equal(expectedResponse.Message,data.Message);
    }
}