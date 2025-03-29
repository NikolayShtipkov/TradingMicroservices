using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceApi.Data;
using PriceApi.Models;

namespace PriceApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PriceController : ControllerBase
    {
        private readonly ILogger<PriceController> _logger;
        private readonly PriceDbContext _context;

        public PriceController(ILogger<PriceController> logger, PriceDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("{ticker}")]
        public async Task<IActionResult> GetPrice(string ticker)
        {
            var price = await _context.Prices.FirstOrDefaultAsync(p => p.Ticker == ticker);
            if (price == null) return NotFound();

            return Ok(new
            {
                ticker = price.Ticker,
                value = price.Value,
                lastUpdated = price.LastUpdated
            });
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddPrice([FromBody] Price price)
        {
            if (await _context.Prices.AnyAsync(p => p.Ticker == price.Ticker))
                return BadRequest("Stock already exists.");

            _context.Prices.Add(price);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPrice), new { ticker = price.Ticker }, price);
        }
    }
}
