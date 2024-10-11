using OrderService.Models;
using RestaurantManagement.SharedDataLibrary.DTOs.Order;
using RestaurantManagement.SharedDataLibrary.Enums;
using RestaurantManagement.SharedLibrary.Responses;

namespace OrderService.Interfaces
{
    public interface IOrderRepository
    {
        Task<Response<OrderResponseDTO>> CreateOrderAsync(CreateOrderRequestDTO orderDto);
        Task<Response<OrderSummaryResponseDTO>> GetOrderSummaryByIdAsync(Guid orderSummaryId);
        Task<Response<List<OrderResponseDTO>>> GetOrdersByUserIdAsync(Guid userId);
        Task<Response<bool>> UpdateOrderItemStatusAsync(UpdateOrderItemStatusDTO updateOrderItemStatusDto);
    }

    //public interface IOrderRepository
    //{
    //    Task<Order> CreateOrderAsync(Order order); // Create a new order
    //    Task<Order> GetOrderByIdAsync(Guid orderId);
    //    Task<List<Order>> GetOrdersByUserIdAsync(Guid userId); // Get orders by user ID
    //    Task<Order> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus); // Update order status
    //    Task UpdateOrderAsync(Order order);
    //    Task CancelOrderAsync(Guid orderId);


    //}
}
