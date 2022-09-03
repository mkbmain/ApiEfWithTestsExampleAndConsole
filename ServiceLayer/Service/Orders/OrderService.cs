using System.Linq.Expressions;
using DataLayer;
using Microsoft.Extensions.Logging;
using ServiceLayer.Models;
using SimpleRepo.Repo;

namespace ServiceLayer.Service.Orders;

public class OrderService : BaseService<DataLayer.Order, OrderService>, IOrderService
{
    public OrderService(IRepo<ExampleDbContext> repo, ILogger<OrderService> orderService) : base(repo, orderService)
    {
    }

    public async Task<ServiceResponse<Guid>> AddOrder(CreateOrderRequest request)
    {
        try
        {
            var customer =
                await Repo.Get<DataLayer.Customer, DataLayer.Customer>(t => t.Email == request.CustomerEmail, t => t);
            if (customer == null)
            {
                return new ServiceResponse<Guid>() {Status = ServiceStatus.BadRequest, Message = "customer not found"};
            }

            var order = new Order
            {
                CustomerId = customer.Id,
                Created = DateTime.Now,
                Total = request.Total
            };
            await Repo.Add(order);
   
            return new ServiceResponse<Guid> {Data = order.Id};
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            return new ServiceResponse<Guid> {Status = ServiceStatus.Error, Message = e.Message};
        }

    }

    public Task<ServiceResponse<OrderResponse>> GetOrdersForCustomer(int customerId) =>
        GetOrdersForCustomer(t => t.Id == customerId);

    public Task<ServiceResponse<OrderResponse>> GetOrdersForCustomer(string customerEmail) =>
        GetOrdersForCustomer(t => t.Email == customerEmail);

    protected async Task<ServiceResponse<OrderResponse>> GetOrdersForCustomer(
        Expression<Func<DataLayer.Customer, bool>> func)
    {
        try
        {
            var item = await Repo.Get(func, t => new OrderResponse
            {
                CustomerId = t.Id,
                Orders = t.Orders.Select(order => new OrderDetails
                {
                    CreatedAt = order.Created,
                    OrderId = order.Id,
                    Total = order.Total
                }).ToList()
            });
            if (item is null)
            {
                return new ServiceResponse<OrderResponse>
                    {Status = ServiceStatus.BadRequest, Message = "No Customer Found"};
            }

            return new ServiceResponse<OrderResponse> {Data = item};
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            return new ServiceResponse<OrderResponse> {Status = ServiceStatus.Error, Message = e.Message};
        }
    }
}