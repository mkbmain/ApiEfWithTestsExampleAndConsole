using ServiceLayer.Models;

namespace ServiceLayer.Service.Customer;

public interface ICustomerService : IBaseService<DataLayer.Customer>
{
     Task<ServiceResponse<int>> AddCustomer(CreateCustomerRequest request);
     Task<ServiceResponse<CustomerResponse>> GetCustomerByEmail(string email);
     Task<ServiceResponse<CustomerResponse>> GetCustomerById(int id);
}