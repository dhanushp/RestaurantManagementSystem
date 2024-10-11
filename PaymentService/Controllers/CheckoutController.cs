using Microsoft.AspNetCore.Mvc;
using PaymentService.Interfaces;
using RestaurantManagement.SharedLibrary.Responses;
using RestaurantManagement.SharedDataLibrary.DTOs.Payment;
using PaymentService.Models;
using RestaurantManagement.SharedDataLibrary.Enums;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly IPaymentRepository _paymentRepository;

        public CheckoutController(ICheckoutRepository checkoutRepository, IPaymentRepository paymentRepository)
        {
            _checkoutRepository = checkoutRepository;
            _paymentRepository = paymentRepository;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] PayPalCreateOrderDTO createOrderDTO)
        {
            try
            {
                var orderResponse = await _checkoutRepository.CreateOrder(createOrderDTO);

                return Ok(Response<PayPalOrderResponseDTO>.SuccessResponse("Order created and payment initialized", orderResponse));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<PayPalOrderResponseDTO>.ErrorResponse(ex));
            }
        }

        [HttpPost("capture-order/{orderId}")]
        public async Task<IActionResult> CaptureOrder(string orderId)
        {
            try
            {
                var captureResponse = await _checkoutRepository.CaptureOrder(orderId);

                // Update the payment status
                var payment = await _paymentRepository.UpdatePaymentStatusAsync(orderId, PaymentStatus.Success);

                return Ok(Response<PayPalCaptureOrderResponseDTO>.SuccessResponse("Order captured and payment completed", captureResponse));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<PayPalCaptureOrderResponseDTO>.ErrorResponse(ex));
            }
        }
    }
}
