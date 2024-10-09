using OrderService.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDTO> CreateOrderAsync(OrderCreateDTO orderCreateDTO); // Create new order
        Task<OrderResponseDTO> UpdateOrderStatusAsync(Guid orderId, OrderStatusUpdateDTO orderStatusUpdateDTO); // Update order status
        Task<bool> CancelOrderAsync(Guid orderId); // Cancel an order
        Task<OrderResponseDTO> GetOrderByIdAsync(Guid orderId); // Get order details by ID
        Task<List<OrderResponseDTO>> GetOrdersByUserIdAsync(Guid userId); // Get all orders for a user
    }
}
