using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderService.DTO;
using OrderService.Interfaces;
using RestaurantManagement.SharedLibrary.Responses;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        // Constructor to inject the OrderService
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Create a new order
        [HttpPost]
        public async Task<ActionResult<OrderResponseDTO>> CreateOrder([FromBody] OrderCreateDTO orderCreateDTO)
        {
            try
            {
                var createdOrder = await _orderService.CreateOrderAsync(orderCreateDTO);
                return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Return error message for bad request
            }
        }

        // Get order by ID
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderById(Guid orderId)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message); // Return not found error if order does not exist
            }
        }

        // Get all orders by user ID
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetOrdersByUserId(Guid userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

       /* // Get order summary by summary ID
        [HttpGet("summary/{summaryId}")]
        public async Task<ActionResult<OrderSummaryDto>> GetOrderSummaryById(Guid summaryId)
        {
            try
            {
                var orderSummary = await _orderService.GetOrderSummaryByIdAsync(summaryId);
                return Ok(orderSummary);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message); // Return not found error if order summary does not exist
            }
        }*/

        // Update order status
        [HttpPut("{orderId}/status")]
        public async Task<ActionResult> UpdateOrderStatus(Guid orderId, [FromBody] OrderStatusUpdateDTO orderStatusUpdateDTO)
        {
            try
            {
                await _orderService.UpdateOrderStatusAsync(orderId, orderStatusUpdateDTO);
                return NoContent(); // Return 204 No Content if successful
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message); // Return not found error if order does not exist
            }
        }

        // Cancel an order
        [HttpDelete("{orderId}")]
        public async Task<ActionResult> CancelOrder(Guid orderId)
        {
            try
            {
                await _orderService.CancelOrderAsync(orderId);
                return NoContent(); // Return 204 No Content if successful
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message); // Return not found error if order does not exist
            }
        }
    }
}
