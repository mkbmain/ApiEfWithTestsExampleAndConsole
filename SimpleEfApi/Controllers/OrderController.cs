using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Models;
using ServiceLayer.Service.Orders;

namespace SimpleEfApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : BaseApiController
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest customerRequest)
    {
        var result = await _orderService.AddOrder(customerRequest);
        return HandleResponse(result, () => Created(result));
    }

    [HttpGet("CustomerId/{customerId}")]
    public async Task<IActionResult> GetByCustomerId([FromRoute] int customerId)
    {
        var result = await _orderService.GetOrdersForCustomer(customerId);
        return HandleResponse(result, () => Ok(result));
    }

    [HttpGet("Email/{customerEmail}")]
    public async Task<IActionResult> GetByEmail([FromRoute] string customerEmail)
    {
        var result = await _orderService.GetOrdersForCustomer(customerEmail);
        return HandleResponse(result, () => Ok(result));
    }
}