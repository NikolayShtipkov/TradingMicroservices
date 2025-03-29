using MassTransit;
using PortfolioApi.Models;
using PortfolioApi.Services.Abstraction;

namespace PortfolioApi.Consumers
{
    public class OrderConsumer : IConsumer<Order>
    {
        private readonly IPortfolioService _service;

        public OrderConsumer(IPortfolioService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<Order> context)
        {
            // Process order and update the portfolio
            // Example: Add the order to the user's portfolio or update their portfolio value.
            var order = context.Message;
            if (order == null)
            {
                return;
            }

            await _service.UpsertPortfolio(order);

            Console.WriteLine($"Received Order - User: {order.UserId}, Ticker: {order.Ticker}, Quantity: {order.Quantity}, Side: {order.Side}, Price: {order.Price}");

            // Update portfolio logic (e.g., save to DB)
        }
    }
}
