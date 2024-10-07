using OrderService.DTO;
using OrderService.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDTO> CreateOrderAsync(OrderCreateDTO orderCreateDTO); // Create new order
        Task<OrderResponseDTO> UpdateOrderStatusAsync(int orderId, OrderStatusUpdateDTO orderStatusUpdateDTO); // Update order status
        Task<bool> CancelOrderAsync(int orderId); // Cancel an order
        Task<OrderResponseDTO> GetOrderByIdAsync(int orderId); // Get order details by ID
        Task<List<OrderResponseDTO>> GetOrdersByUserIdAsync(int userId); // Get all orders for a user
    }
}
