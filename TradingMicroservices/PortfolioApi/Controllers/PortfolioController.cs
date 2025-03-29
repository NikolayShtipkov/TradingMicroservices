using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Services.Abstraction;

namespace PortfolioApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly ILogger<PortfolioController> _logger;
        private readonly IPortfolioService _service;

        public PortfolioController(ILogger<PortfolioController> logger, IPortfolioService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetPortfolio(string userId)
        {
            var portfolio = await _service.GetPortfolio(userId);

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
