using Microsoft.AspNetCore.Mvc;
using OrderApi.Models;
using OrderApi.Services.Abstraction;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost("add/{userId}")]
        public async Task<IActionResult> PlaceOrder(string userId, [FromBody] OrderDto order)
        {
            if (order == null) return BadRequest("Invalid order data");

            await _service.PlaceOrder(order, userId);

            return CreatedAtAction(nameof(GetOrder), new { uId = userId }, order);
        }

        [HttpGet()]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _service.GetOrders();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _service.GetOrder(id);
            if (order == null) return NotFound();

            return Ok(order);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserOrders(string userId)
        {
            var orders = await _service.GetUserOrders(userId);
            return Ok(orders);
        }
    }
}
