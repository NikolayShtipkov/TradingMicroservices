using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Models;
using PortfolioApi.Services.Abstraction;

namespace PortfolioApi.Services
{
    internal class PortfolioService : IPortfolioService
    {
        private readonly PortfolioDbContext _context;

        public PortfolioService(PortfolioDbContext context)
        {
            _context = context;
        }

        public async Task<Portfolio?> GetPortfolio(string userId)
        {
            var portfolio = await _context.Portfolios
                .Include(p => p.Holdings)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            return portfolio;
        }

        public async Task UpsertPortfolio(Order order)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var portfolio = await _context.Portfolios
                    .Include(p => p.Holdings)
                    .FirstOrDefaultAsync(p => p.UserId == order.UserId);

                if (portfolio == null)
                {
                    portfolio = new Portfolio
                    {
                        UserId = order.UserId,
                        TotalValue = 0,
                        Holdings = new List<PortfolioHolding>()
                    };

                    _context.Portfolios.Add(portfolio);
                    await _context.SaveChangesAsync();
                }

                var holding = portfolio.Holdings.FirstOrDefault(h => h.Ticker == order.Ticker);

                if (holding == null)
                {
                    if (order.Side == "sell")
                    {
                        throw new Exception("Cannot sell stock that is not owned.");
                    }

                    holding = new PortfolioHolding
                    {
                        PortfolioId = portfolio.Id,
                        Ticker = order.Ticker,
                        Quantity = order.Quantity
                    };

                    portfolio.Holdings.Add(holding);
                }
                else
                {
                    if (order.Side == "buy")
                    {
                        holding.Quantity += order.Quantity;
                    }
                    else if (order.Side == "sell")
                    {
                        if (holding.Quantity < order.Quantity)
                        {
                            throw new Exception("Not enough shares to sell.");
                        }

                        holding.Quantity -= order.Quantity;

                        if (holding.Quantity == 0)
                        {
                            _context.PortfolioHoldings.Remove(holding);
                        }
                    }
                }

                portfolio.TotalValue = await CalculateTotalPortfolioValue(portfolio.Id);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<decimal> CalculateTotalPortfolioValue(int portfolioId)
        {
            return await _context.PortfolioHoldings
                .Where(h => h.PortfolioId == portfolioId)
                .SumAsync(h => h.Quantity * GetLatestPrice(h.Ticker!)); // Assume GetLatestPrice fetches price
        }

        // This method simulates getting the latest stock price
        private decimal GetLatestPrice(string ticker)
        {
            // Replace with actual logic (e.g., cache, DB, or API call)
            return new Random().Next(50, 300); // Mock prices between 50-300
        }
    }
}
