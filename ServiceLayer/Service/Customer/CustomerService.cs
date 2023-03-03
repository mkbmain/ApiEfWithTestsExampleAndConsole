using System.Linq.Expressions;
using DataLayer;
using Microsoft.Extensions.Logging;
using ServiceLayer.Models;
using SimpleRepo.Repo;

namespace ServiceLayer.Service.Customer;

public class CustomerService : BaseService<DataLayer.Customer, CustomerService>, ICustomerService
{
    public CustomerService(IRepo<CustomerOrdersDbContext> repo, ILogger<CustomerService> logger) : base(repo, logger)
    {
    }

    public Task<ServiceResponse<CustomerResponse>> GetCustomerByEmail(string email) =>
        CustomerResponseByX(t => t.Email == email); // adding the async await keywords here can be done but builds a state engine with a move next when not really needed for 1 liner just return the task next await will catch it


    public Task<ServiceResponse<CustomerResponse>> GetCustomerById(int id) => CustomerResponseByX(t => t.Id == id);


    protected async Task<ServiceResponse<CustomerResponse>> CustomerResponseByX(
        Expression<Func<DataLayer.Customer, bool>> func)
    {
        try
        {
            var item = await Repo.Get(func, arg => new CustomerResponse
            {
                Dob = arg.DateOfBirth,
                Email = arg.Email,
                Id = arg.Id,
                Name = arg.Name
            });

            if (item is null)
            {
                return new ServiceResponse<CustomerResponse>
                    {Status = ServiceStatus.BadRequest, Message = "User Not Found"};
            }

            return new ServiceResponse<CustomerResponse> {Status = ServiceStatus.Success, Data = item};
        }
        catch (Exception e)
        {
            // TODO LOG
            Logger.LogError(e, e.Message);
            return new ServiceResponse<CustomerResponse>() {Status = ServiceStatus.Error, Message = e.Message};
        }
    }

    public async Task<ServiceResponse<int>> AddCustomer(CreateCustomerRequest request)
    {
        try
        {
            var user = await GetCustomerByEmail(request.Email);
            if (user.Status == ServiceStatus.Success)
            {
                return new ServiceResponse<int>() {Status = ServiceStatus.BadRequest, Message = "Email already in use"};
            }
            var customer = new DataLayer.Customer
            {
                Email = request.Email,
                DateOfBirth = request.Dob,
                Name = request.Name,
            };
            customer.CustomerAddresses.Add(new CustomerAddress
            {
                BuildingName = request.BuildingName,
                BuildingNumber = request.BuildingNumber,
                Street = request.Street,
                City = request.City,
                PostCode = request.PostCode
            });
            await Repo.Add(customer);
            return new ServiceResponse<int> {Data = customer.Id};
        }
        catch (Exception e)
        {          
            Logger.LogError(e, e.Message);
            return new ServiceResponse<int> {Status = ServiceStatus.Error, Message = e.Message};
        }
    }
}