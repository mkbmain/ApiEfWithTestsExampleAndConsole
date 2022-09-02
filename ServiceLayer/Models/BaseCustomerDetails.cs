namespace ServiceLayer.Models;

public abstract class BaseCustomerDetails
{
    public string Name { get; set; }
    public DateTime Dob { get; set; }
    public string Email { get; set; }
}