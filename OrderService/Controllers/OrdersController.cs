using Microsoft.AspNetCore.Mvc;
using OrderService.Interfaces;

namespace OrderService.Controllers
{
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        // Constructor to inject the OrderService
        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
    }
}
