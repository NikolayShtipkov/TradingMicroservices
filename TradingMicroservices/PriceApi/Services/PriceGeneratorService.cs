using Microsoft.EntityFrameworkCore;
using PriceApi.Data;

namespace PriceApi.Services
{
    public class PriceGeneratorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly Random _random = new();

        public PriceGeneratorService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<PriceDbContext>();
                    var stocks = await _context.Prices.ToListAsync();

                    foreach (var stock in stocks)
                    {
                        stock.Value = (decimal)(_random.NextDouble() * (300 - 50) + 50);
                        stock.LastUpdated = DateTime.UtcNow;
                    }

                    await _context.SaveChangesAsync();
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
