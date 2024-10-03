using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderContext _context;

        public OrderController(OrderContext context)
        {
            _context = context;
        }

        // GET: api/order/user/{userId} - Get all orders for a specific user
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserOrders(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return Ok(orders);
        }

        // POST: api/order - Place a new order
        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] Order order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Validate input

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return Ok("Order placed successfully");
        }

        // PUT: api/order/{id} - Update the status of an existing order
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            await _context.SaveChangesAsync();
            return Ok("Order status updated");
        }

        // POST: api/order/pay/{orderId} - Handle payment for an order
        [HttpPost("pay/{orderId}")]
        public async Task<IActionResult> PayForOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            // Mark order as Paid
            order.Status = OrderStatus.Paid;
            await _context.SaveChangesAsync();

            // Redirect the user to leave a review after payment
            return Redirect($"https://your-frontend-app.com/review/{orderId}");
        }
    }
}
