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
        private readonly ILogger<OrderService> _logger;

        public OrderService(OrderDbContext context, IPublishEndpoint publishEndpoint, ILogger<OrderService> logger)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task PlaceOrder(OrderDto orderDto, string userId)
        {
            Order order = MapModel(orderDto, userId);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            OrderMessage message = MapToMessage(order);

            try
            {
                await _publishEndpoint.Publish<OrderMessage>(message);
                _logger.LogInformation($"Published order message for user {userId}, ticker {order.Ticker}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to publish order message for user {userId}");
                throw;
            }
        }

        private static OrderMessage MapToMessage(Order order)
        {
            return new OrderMessage
            {
                UserId = order.UserId,
                Ticker = order.Ticker,
                Quantity = order.Quantity,
                Side = order.Side,
                Price = order.Price,
                CreatedAt = order.CreatedAt
            };
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
