using MenuService.Interfaces;
using OrderService.DTO;
using OrderService.Interfaces;
using OrderService.Models;
using RestaurantManagement.SharedLibrary.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Interfaces;
using UserService.DTOs;
using UserService.Models;

namespace OrderService.Repositories
{
    public class OrderServiced : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUser _userHttpClient; // Injecting UserHttpClient
        private readonly IMenuItem _menuItemHttpClient; // Injecting MenuItemHttpClient

        public OrderServiced(IOrderRepository orderRepository, IUser userHttpClient, IMenuItem menuItemHttpClient)
        {
            _orderRepository = orderRepository;
            _userHttpClient = userHttpClient;
            _menuItemHttpClient = menuItemHttpClient;
        }

        public async Task<OrderResponseDTO> CreateOrderAsync(OrderCreateDTO orderCreateDTO)
        {
            // Fetch user details
            var userResponse = await _userHttpClient.GetUserById(orderCreateDTO.UserId);
            if (!userResponse.Success) // Assuming your Response class has IsSuccess property
                throw new Exception("User not found");

            // Map DTO to Order model
            var order = new Order
            {
                
                OrderItems = new List<OrderItem>()
            };

            // Validate and fetch menu items
            foreach (var item in orderCreateDTO.OrderItems)
            {
                var menuItemResponse = await _menuItemHttpClient.GetMenuItemById(item.MenuItemId);
                if (!menuItemResponse.Success) // Check if the menu item is valid
                    throw new Exception($"Menu item with ID {item.MenuItemId} not found");

                // Map the valid menu item to OrderItem
                order.OrderItems.Add(new OrderItem
                {
                    MenuItemId = menuItemResponse.Data.Id,
                    MenuItemName = menuItemResponse.Data.Name,
                    MenuItemPrice = menuItemResponse.Data.Price,
                    Quantity = item.Quantity
                });
            }

            order.Status = OrderStatus.Pending; // Default status when creating an order

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

        public async Task<OrderResponseDTO> UpdateOrderStatusAsync(Guid orderId, OrderStatusUpdateDTO orderStatusUpdateDTO)
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

        public async Task<bool> CancelOrderAsync(Guid orderId) // Changed int to Guid
        {
            return await _orderRepository.CancelOrderAsync(orderId);
        }

        public async Task<OrderResponseDTO> GetOrderByIdAsync(Guid orderId)
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

        public async Task<List<OrderResponseDTO>> GetOrdersByUserIdAsync(Guid userId) // Changed int to Guid
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
