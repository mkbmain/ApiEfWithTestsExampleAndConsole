namespace ServiceLayer.Models;

public class ServiceResponse
{
    public string Message { get; set; }
    public ServiceStatus Status { get; set; } = ServiceStatus.Success;
}

public class ServiceResponse<T> : ServiceResponse
{
    public T Data { get; set; }
}