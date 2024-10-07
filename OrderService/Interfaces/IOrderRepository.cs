using OrderService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order); // Create a new order
        Task<Order> GetOrderByIdAsync(int orderId); // Get order by ID
        Task<List<Order>> GetOrdersByUserIdAsync(int userId); // Get orders by user ID
        Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus); // Update order status
        Task<bool> CancelOrderAsync(int orderId); // Cancel an order
    }
}
