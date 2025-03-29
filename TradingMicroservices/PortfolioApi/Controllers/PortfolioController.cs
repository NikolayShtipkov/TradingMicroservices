using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;

namespace PortfolioApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly ILogger<PortfolioController> _logger;
        private readonly PortfolioDbContext _context;

        public PortfolioController(ILogger<PortfolioController> logger, PortfolioDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetPortfolio(string userId)
        {
            var portfolio = await _context.Portfolios
                .Include(p => p.Holdings)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (portfolio == null) return NotFound();

            return Ok(new
            {
                userId = portfolio.UserId,
                totalValue = portfolio.TotalValue,
                holdings = portfolio.Holdings.Select(h => new { h.Ticker, h.Quantity })
            });
        }
    }
}
