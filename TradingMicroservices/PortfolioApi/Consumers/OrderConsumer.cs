using MassTransit;
using PortfolioApi.Models;
using PortfolioApi.Services.Abstraction;

namespace PortfolioApi.Consumers
{
    public class OrderConsumer : IConsumer<OrderMessage>
    {
        private readonly IPortfolioService _service;

        public OrderConsumer(IPortfolioService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<OrderMessage> context)
        {
            var message = context.Message;
            if (message == null)
            {
                return;
            }

            Order order = MapToOrder(message);

            await _service.UpsertPortfolio(order);

            Console.WriteLine($"Received Order - User: {message.UserId}, Ticker: {message.Ticker}, Quantity: {message.Quantity}, Side: {message.Side}, Price: {message.Price}");

            // Update portfolio logic (e.g., save to DB)
        }

        private static Order MapToOrder(OrderMessage order)
        {
            return new Order
            {
                UserId = order.UserId,
                Ticker = order.Ticker,
                Quantity = order.Quantity,
                Side = order.Side,
                Price = order.Price,
                CreatedAt = order.CreatedAt
            };
        }
    }
}
