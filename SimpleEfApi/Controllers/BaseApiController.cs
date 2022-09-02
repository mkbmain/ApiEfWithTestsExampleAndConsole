using System.Net;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Models;

namespace SimpleEfApi.Controllers;

public class BaseApiController : ControllerBase
{
    protected static IActionResult HandleResponse<T>(ServiceResponse<T> serviceResponse, Func<IActionResult> goodAction)
    {
        switch (serviceResponse.Status)
        {
            case ServiceStatus.Error:
                return new ObjectResult(null) {StatusCode = (int) HttpStatusCode.InternalServerError};
            case ServiceStatus.BadRequest:
                return new ObjectResult(serviceResponse.Message) {StatusCode = (int) HttpStatusCode.BadRequest};
                break;
            case ServiceStatus.Success:
                return goodAction.Invoke();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}