using OrderService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order); // Create a new order
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<List<Order>> GetOrdersByUserIdAsync(Guid userId); // Get orders by user ID
        Task<Order> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus); // Update order status
        Task UpdateOrderAsync(Order order);
        Task CancelOrderAsync(Guid orderId);


    }
}
