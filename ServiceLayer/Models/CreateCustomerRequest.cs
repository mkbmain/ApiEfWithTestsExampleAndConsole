namespace ServiceLayer.Models;

public class CreateCustomerRequest : BaseCustomerDetails
{
    public string PostCode { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string BuildingNumber { get; set; } = null!;
    public string BuildingName { get; set; } = null!;
    public string City { get; set; } = null!;
}