using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Interfaces;
using PaymentService.Models;
using RestaurantManagement.SharedLibrary.Responses;
using RestaurantManagement.SharedDataLibrary.DTOs.Payment;
using RestaurantManagement.SharedDataLibrary.Enums;
using Microsoft.AspNetCore.Authorization;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        // POST: api/payment/process
        [HttpPost("process-cash")]
        public async Task<IActionResult> ProcessCashPayment([FromBody] PaymentCashDTO paymentDTO)
        {
            try
            {
                // Validate input
                if (!ModelState.IsValid)
                    return BadRequest(Response<PaymentCashDTO>.ErrorResponse("Invalid payment details"));

                // Call repository to add payment
                var payment = new Payment
                {
                    UserId = paymentDTO.UserId,
                    FoodOrderId = paymentDTO.FoodOrderId,
                    Amount = paymentDTO.Amount,
                    CurrencyCode = paymentDTO.CurrencyCode,
                    PaymentMethod = PaymentMethod.Cash,
                    Status = PaymentStatus.Success
                };

                var processedPayment = await _paymentRepository.AddPaymentAsync(payment);
                return Ok(Response<Payment>.SuccessResponse("Payment processed successfully", processedPayment));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<PaymentCashDTO>.ErrorResponse(ex));
            }
        }

        // GET: api/payment/order/{orderId}
        //[HttpGet("order/{orderId}")]
        //public async Task<IActionResult> GetPaymentByFoodOrder(Guid orderId)
        //{
        //    try
        //    {
        //        var payments = await _paymentRepository.GetPaymentByFoodOrderIdAsync(orderId);
        //        if (!payments.Any())
        //            return NotFound(Response<IEnumerable<Payment>>.ErrorResponse("No payments found for the order"));

        //        return Ok(Response<IEnumerable<Payment>>.SuccessResponse("Payments retrieved", payments));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(Response<IEnumerable<Payment>>.ErrorResponse(ex));
        //    }
        //}
    }
}
