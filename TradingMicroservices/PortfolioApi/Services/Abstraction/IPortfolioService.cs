using PortfolioApi.Models;

namespace PortfolioApi.Services.Abstraction
{
    public interface IPortfolioService
    {
        Task<Portfolio?> GetPortfolio(string userId);
        Task UpsertPortfolio(Order portfolio);
    }
}