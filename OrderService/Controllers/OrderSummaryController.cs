using Microsoft.AspNetCore.Mvc;
using OrderService.Interfaces;
using OrderService.DTO;
using System;
using System.Threading.Tasks;

namespace OrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderSummaryController : ControllerBase
    {
        private readonly IOrderSummaryService _orderSummaryService;

        public OrderSummaryController(IOrderSummaryService orderSummaryService)
        {
            _orderSummaryService = orderSummaryService;
        }

        // GET: api/OrderSummary/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderSummaryDto>> GetOrderSummaryById(Guid id)
        {
            var orderSummary = await _orderSummaryService.GetOrderSummaryByIdAsync(id);
            if (orderSummary == null)
            {
                return NotFound(new { Message = "Order summary not found" });
            }
            return Ok(orderSummary);
        }

        // POST: api/OrderSummary
        [HttpPost]
        public async Task<ActionResult<CreateOrderSummaryDto>> CreateOrderSummary(CreateOrderSummaryDto createOrderSummaryDto)
        {
            await _orderSummaryService.CreateOrderSummaryAsync(createOrderSummaryDto);

            // Assuming you have a method to retrieve the newly created summary
            var createdOrderSummary = await _orderSummaryService.GetOrderSummaryByIdAsync(createOrderSummaryDto.Orders.First().UserId); // Adjust this to retrieve the summary correctly

            return CreatedAtAction(nameof(GetOrderSummaryById), new { id = createdOrderSummary.Id }, createdOrderSummary);
        }
    }
}
