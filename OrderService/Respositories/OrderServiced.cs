using OrderService.DTO;
using OrderService.DTOs;
using OrderService.Interfaces;
using OrderService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Respositories
{
    public class OrderServiced : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderServiced(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderResponseDTO> CreateOrderAsync(OrderCreateDTO orderCreateDTO)
        {
            // Map DTO to Order model
            var order = new Order
            {
                UserId = orderCreateDTO.UserId,
                OrderItems = orderCreateDTO.OrderItems.Select(item => new OrderItem
                {
                    MenuItemId = item.MenuItemId,
                    MenuItemName = item.MenuItemName,
                    MenuItemPrice = item.MenuItemPrice,
                    Quantity = item.Quantity
                }).ToList(),
                Status = OrderStatus.Pending // Default status when creating an order
            };

            // Create the order and save to the repository
            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            // Prepare response DTO
            return new OrderResponseDTO
            {
                Id = createdOrder.Id,
                UserId = createdOrder.UserId,
                OrderItems = createdOrder.OrderItems.Select(item => new OrderItemResponseDTO
                {
                    MenuItemId = item.MenuItemId,
                    MenuItemName = item.MenuItemName,
                    MenuItemPrice = item.MenuItemPrice,
                    Quantity = item.Quantity
                }).ToList(),
                Status = createdOrder.Status.ToString(),
                TotalPrice = createdOrder.OrderItems.Sum(item => item.MenuItemPrice * item.Quantity),
                CreatedAt = createdOrder.CreatedAt,
                UpdatedAt = createdOrder.UpdatedAt
            };
        }

        public async Task<OrderResponseDTO> UpdateOrderStatusAsync(int orderId, OrderStatusUpdateDTO orderStatusUpdateDTO)
        {
            var updatedOrder = await _orderRepository.UpdateOrderStatusAsync(orderId, orderStatusUpdateDTO.NewStatus);
            if (updatedOrder == null) return null;

            // Map updated order to DTO
            return new OrderResponseDTO
            {
                Id = updatedOrder.Id,
                UserId = updatedOrder.UserId,
                OrderItems = updatedOrder.OrderItems.Select(item => new OrderItemResponseDTO
                {
                    MenuItemId = item.MenuItemId,
                    MenuItemName = item.MenuItemName,
                    MenuItemPrice = item.MenuItemPrice,
                    Quantity = item.Quantity
                }).ToList(),
                Status = updatedOrder.Status.ToString(),
                TotalPrice = updatedOrder.OrderItems.Sum(item => item.MenuItemPrice * item.Quantity),
                CreatedAt = updatedOrder.CreatedAt,
                UpdatedAt = updatedOrder.UpdatedAt
            };
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            return await _orderRepository.CancelOrderAsync(orderId);
        }

        public async Task<OrderResponseDTO> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) return null;

            // Map order to DTO
            return new OrderResponseDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderItems = order.OrderItems.Select(item => new OrderItemResponseDTO
                {
                    MenuItemId = item.MenuItemId,
                    MenuItemName = item.MenuItemName,
                    MenuItemPrice = item.MenuItemPrice,
                    Quantity = item.Quantity
                }).ToList(),
                Status = order.Status.ToString(),
                TotalPrice = order.OrderItems.Sum(item => item.MenuItemPrice * item.Quantity),
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            };
        }

        public async Task<List<OrderResponseDTO>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return orders.Select(order => new OrderResponseDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderItems = order.OrderItems.Select(item => new OrderItemResponseDTO
                {
                    MenuItemId = item.MenuItemId,
                    MenuItemName = item.MenuItemName,
                    MenuItemPrice = item.MenuItemPrice,
                    Quantity = item.Quantity
                }).ToList(),
                Status = order.Status.ToString(),
                TotalPrice = order.OrderItems.Sum(item => item.MenuItemPrice * item.Quantity),
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            }).ToList();
        }
    }
}
