using ServiceLayer.Models;

namespace ServiceLayer.Service.Orders;

public interface IOrderService : IBaseService<DataLayer.Order>
{
    public Task<ServiceResponse<Guid>> AddOrder(CreateOrderRequest request);

    public Task<ServiceResponse<OrderResponse>> GetOrdersForCustomer(int customerId);
    
    public Task<ServiceResponse<OrderResponse>> GetOrdersForCustomer(string customerEmail);
}