namespace ServiceLayer.Models;

public class CreateOrderRequest
{
    public string CustomerEmail { get; set; }
    public decimal Total { get; set; }
}