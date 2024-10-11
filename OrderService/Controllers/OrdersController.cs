using Microsoft.AspNetCore.Mvc;
using OrderService.Interfaces;
using OrderService.Repositories;
using RestaurantManagement.SharedDataLibrary.DTOs.Order;
using RestaurantManagement.SharedLibrary.Responses;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDTO orderDto)
        {
            var result = await _orderRepository.CreateOrderAsync(orderDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("summary/{orderSummaryId}")]
        public async Task<IActionResult> GetOrderSummaryById(Guid orderSummaryId)
        {
            var result = await _orderRepository.GetOrderSummaryByIdAsync(orderSummaryId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(Guid userId)
        {
            var result = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPatch]
        [Route("orderitem/status")]
        public async Task<IActionResult> UpdateOrderItemStatus([FromBody] UpdateOrderItemStatusDTO updateOrderItemStatusDto)
        {
            var result = await _orderRepository.UpdateOrderItemStatusAsync(updateOrderItemStatusDto);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
