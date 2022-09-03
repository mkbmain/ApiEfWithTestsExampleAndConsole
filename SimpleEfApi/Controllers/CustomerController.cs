using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Models;
using ServiceLayer.Service.Customer;

namespace SimpleEfApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : BaseApiController
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateCustomerRequest customerRequest)
    {
        var result = await _customerService.AddCustomer(customerRequest);
        return HandleResponse(result, () => Created(result));
    }

    [HttpGet("Id/{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        var result = await _customerService.GetCustomerById(id);
        return HandleResponse(result, () => Ok(result));
    }

    [HttpGet("Email/{email}")]
    public async Task<IActionResult> GetUserByEmail([FromRoute] string email)

    {
        var result = await _customerService.GetCustomerByEmail(email);
        return HandleResponse(result, () => Ok(result));
    }
}