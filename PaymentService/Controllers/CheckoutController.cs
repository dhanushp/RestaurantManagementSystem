using Microsoft.AspNetCore.Mvc;
using PaymentService.Interfaces;
using RestaurantManagement.SharedDataLibrary.DTOs.Payment;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutRepository _checkoutRepository;

        public CheckoutController(ICheckoutRepository checkoutRepository)
        {
            _checkoutRepository = checkoutRepository;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] PayPalCreateOrderDTO createOrderDTO)
        {
            try
            {
                var orderResponse = await _checkoutRepository.CreateOrder(createOrderDTO);
                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("capture-order/{orderId}")]
        public async Task<IActionResult> CaptureOrder(string orderId)
        {
            try
            {
                var captureResponse = await _checkoutRepository.CaptureOrder(orderId);
                return Ok(captureResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

}
