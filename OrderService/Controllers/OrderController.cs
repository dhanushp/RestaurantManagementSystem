﻿using Microsoft.AspNetCore.Mvc;
using OrderService.DTO;
using OrderService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Create a new order.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<OrderResponseDTO>> CreateOrder([FromBody] OrderCreateDTO orderCreateDTO)
        {
            if (orderCreateDTO == null)
            {
                return BadRequest("Invalid order data.");
            }

            var createdOrder = await _orderService.CreateOrderAsync(orderCreateDTO);
            return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.Id }, createdOrder);
        }

        /// <summary>
        /// Update the status of an existing order.
        /// </summary>
        [HttpPut("{orderId}/status")]
        public async Task<ActionResult<OrderResponseDTO>> UpdateOrderStatus(Guid orderId, [FromBody] OrderStatusUpdateDTO orderStatusUpdateDTO) // Changed int to Guid
        {
            if (orderStatusUpdateDTO == null)
            {
                return BadRequest("Invalid status data.");
            }

            var updatedOrder = await _orderService.UpdateOrderStatusAsync(orderId, orderStatusUpdateDTO);
            if (updatedOrder == null)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }

            return Ok(updatedOrder);
        }

        /// <summary>
        /// Cancel an existing order.
        /// </summary>
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> CancelOrder(Guid orderId) // Changed int to Guid
        {
            var isCancelled = await _orderService.CancelOrderAsync(orderId);
            if (!isCancelled)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }

            return NoContent(); // 204 No Content
        }

        /// <summary>
        /// Get details of a specific order by ID.
        /// </summary>
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderById(Guid orderId) // Changed int to Guid
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }

            return Ok(order);
        }

        /// <summary>
        /// Get all orders for a specific user.
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetOrdersByUserId(Guid userId) // Changed int to Guid
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }
    }
}
