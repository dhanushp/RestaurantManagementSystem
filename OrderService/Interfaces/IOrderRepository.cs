using OrderService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order); // Create a new order
        Task<Order> GetOrderByIdAsync(Guid orderId); // Get order by ID
        Task<List<Order>> GetOrdersByUserIdAsync(Guid userId); // Get orders by user ID
        Task<Order> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus); // Update order status
        Task<bool> CancelOrderAsync(Guid orderId); // Cancel an order
    }
}
