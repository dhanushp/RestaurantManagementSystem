using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentContext _context;

        public PaymentController(PaymentContext context)
        {
            _context = context;
        }

        // POST: api/payment/process - Process a new payment
        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] Payment payment)
        {
            // Make an API call to OrderService to verify the order
            var orderExists = await VerifyOrderExists(payment.OrderId);
            if (!orderExists)
                return NotFound("Order not found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Validate input

            payment.Status = PaymentStatus.Pending; // Set initial status to Pending
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return Ok("Payment processed successfully");
        }

        // Helper method to call OrderService and verify if the order exists
        private async Task<bool> VerifyOrderExists(int orderId)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"https://order-service/api/order/{orderId}");
                return response.IsSuccessStatusCode;
            }
        }

        // GET: api/payment/order/{orderId} - Retrieve payment by orderId
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentByOrder(int orderId)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }
    }
}
