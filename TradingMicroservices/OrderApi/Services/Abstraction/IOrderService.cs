using OrderApi.Models;

namespace OrderApi.Services.Abstraction
{
    public interface IOrderService
    {
        Task<Order?> GetOrder(int id);
        Task<IEnumerable<Order>> GetOrders();
        Task<IEnumerable<Order>> GetUserOrders(string userId);
        Task PlaceOrder(OrderDto orderDto, string userId);
    }
}