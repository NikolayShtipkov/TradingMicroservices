using MassTransit;
using PortfolioApi.Models;

namespace PortfolioApi.Consumers
{
    public class OrderCreatedConsumer : IConsumer<Order>
    {
        public async Task Consume(ConsumeContext<Order> context)
        {
            var order = context.Message;

            // Process order and update the portfolio
            // Example: Add the order to the user's portfolio or update their portfolio value.

            Console.WriteLine($"Received Order - User: {order.UserId}, Ticker: {order.Ticker}, Quantity: {order.Quantity}, Side: {order.Side}, Price: {order.Price}");

            // Update portfolio logic (e.g., save to DB)
        }
    }
}
