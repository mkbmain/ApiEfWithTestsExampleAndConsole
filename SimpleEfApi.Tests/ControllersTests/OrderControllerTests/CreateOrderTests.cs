using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ServiceLayer.Models;
using Xunit;

namespace SimpleEfApi.Tests.ControllersTests.OrderControllerTests;

public class CreateOrderTests
{
    [Fact]
    public async Task Ensure_on_success_we_report()
    {
        var request = OrderRequest;
        var (sut, mockOrderService) = OrderControllerFactory.Build();
        var expectedResponse = new ServiceResponse<Guid>() {Status = ServiceStatus.Success, Data = Guid.NewGuid()};
        mockOrderService.Setup(e => e.AddOrder(request))
            .ReturnsAsync(expectedResponse);

        var result = await sut.CreateOrder(request);
        var response = result as ObjectResult;
        Assert.Equal((int) HttpStatusCode.Created, response.StatusCode);
        var data = response.Value as ServiceResponse<Guid>;
        Assert.Equal(expectedResponse, data);
    }

    [Fact]
    public async Task Ensure_on_error_we_report()
    {
        var request = OrderRequest;
        var (sut, mockOrderService) = OrderControllerFactory.Build();
        var expectedResponse = new ServiceResponse<Guid>()
            {Status = ServiceStatus.Error, Message = "test", Data = Guid.NewGuid()};
        mockOrderService.Setup(e => e.AddOrder(request))
            .ReturnsAsync(expectedResponse);

        var result = await sut.CreateOrder(request);
        var response = result as ObjectResult;
        Assert.Equal((int) HttpStatusCode.InternalServerError, response.StatusCode);
        var data = response.Value as ServiceResponse;
        Assert.Equal(expectedResponse.Status, data.Status);
        Assert.Empty(data.Message);
    }
    
    [Fact]
    public async Task Ensure_on_badrequest_we_report()
    {
        var request = OrderRequest;
        var (sut, mockOrderService) = OrderControllerFactory.Build();
        var expectedResponse = new ServiceResponse<Guid>()
            {Status = ServiceStatus.BadRequest, Message = "test", Data = Guid.NewGuid()};
        mockOrderService.Setup(e => e.AddOrder(request))
            .ReturnsAsync(expectedResponse);

        var result = await sut.CreateOrder(request);
        var response = result as ObjectResult;
        Assert.Equal((int) HttpStatusCode.BadRequest, response.StatusCode);
        var data = response.Value as ServiceResponse;
        Assert.Equal(expectedResponse.Status, data.Status);
        Assert.Equal(expectedResponse.Message,data.Message);
    }

    private static CreateOrderRequest OrderRequest = new CreateOrderRequest
        {Total = 532, CustomerEmail = "test@te.com"};
}