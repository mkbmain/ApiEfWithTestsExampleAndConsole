using System.Net;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Models;

namespace SimpleEfApi.Controllers;

public abstract class BaseApiController : ControllerBase
{
    protected static IActionResult Created<T>(T ob) where T : ServiceResponse =>
        new ObjectResult(ob) {StatusCode = (int) HttpStatusCode.Created};


    protected static IActionResult HandleResponse<T>(ServiceResponse<T> serviceResponse, Func<IActionResult> goodAction)
    {
        switch (serviceResponse.Status)
        {
            case ServiceStatus.BadRequest:
                return new ObjectResult(new ServiceResponse
                        {Message = serviceResponse.Message, Status = serviceResponse.Status})
                    {StatusCode = (int) HttpStatusCode.BadRequest};
            case ServiceStatus.Success:
                return goodAction.Invoke();
            case ServiceStatus.Error:
            default:
                return new ObjectResult(new ServiceResponse {Message = "", Status = serviceResponse.Status})
                    {StatusCode = (int) HttpStatusCode.InternalServerError};
        }
    }
}