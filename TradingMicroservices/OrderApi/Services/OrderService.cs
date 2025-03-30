using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Models;
using OrderApi.Services.Abstraction;

namespace OrderApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderService(OrderDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task PlaceOrder(OrderDto orderDto, string userId)
        {
            Order order = MapModel(orderDto, userId);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            await _publishEndpoint.Publish(order);
        }

        public async Task<Order?> GetOrder(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetUserOrders(string userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        private static Order MapModel(OrderDto orderDto, string userId)
        {
            return new()
            {
                UserId = userId.ToString(),
                Ticker = orderDto.Ticker,
                Quantity = orderDto.Quantity,
                Side = orderDto.Side,
                Price = orderDto.Price
            };
        }
    }
}
