using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq; // Add this for LINQ operations
using MenuService.DTOs;
using OrderService.Interfaces;
using RestaurantManagement.SharedLibrary.Responses;
using OrderService.Models;
using OrderService.DTO;
using OrderService.Repositories;
using UserService.DTOs;

public class OrderServiced : IOrderService
{
    private IHttpClientFactory _httpClientFactory;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderSummaryRepository _orderSummaryRepository;

    private readonly string _menuServiceUrl = "http://localhost:5003/api/menuitems"; // Change to actual URL
    private readonly string _userServiceUrl = "http://localhost:5003/api/users"; // Change to actual URL

    // Constructor
    public OrderServiced(
        IHttpClientFactory httpClientFactory,
        IOrderRepository orderRepository,
        IOrderSummaryRepository orderSummaryRepository)
    {
        _httpClientFactory = httpClientFactory;
        _orderRepository = orderRepository;
        _orderSummaryRepository = orderSummaryRepository;
    }

    public async Task<OrderResponseDTO> CreateOrderAsync(OrderCreateDTO orderCreateDTO)
    {
        // Fetch user details from UserService
        var userClient = _httpClientFactory.CreateClient();
        var userResponse = await userClient.GetAsync($"{_userServiceUrl}/{orderCreateDTO.UserId}");

        if (!userResponse.IsSuccessStatusCode)
        {
            throw new Exception("User not found.");
        }

        var userDetails = await userResponse.Content.ReadFromJsonAsync<UserDTO>();

        // Fetch menu item details and create order items
        var orderItems = new List<OrderItem>();
        foreach (var item in orderCreateDTO.OrderItems)
        {
            var menuResponse = await userClient.GetAsync($"{_menuServiceUrl}/{item.MenuItemId}");

            if (!menuResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Menu item {item.MenuItemId} not found.");
            }

            var menuItem = await menuResponse.Content.ReadFromJsonAsync<MenuItemDetailResponseDTO>();
            orderItems.Add(new OrderItem
            {
                MenuItemId = menuItem.Id,
                MenuItemName = menuItem.Name,
                Quantity = item.Quantity,
                MenuItemPrice = menuItem.Price
            });
        }

        // Check if an order summary exists for this user
        OrderSummary orderSummary;
        if (orderCreateDTO.OrderSummaryId == null)
        {
            // Create a new order summary
            orderSummary = new OrderSummary
            {
                TableNumber = orderCreateDTO.TableNumber,
                Orders = new List<Order>() // Initialize list of orders
            };
            await _orderSummaryRepository.AddAsync(orderSummary);
        }
        else
        {
            // Fetch existing order summary
            orderSummary = await _orderSummaryRepository.GetOrderSummaryByIdAsync(orderCreateDTO.OrderSummaryId.Value);
            if (orderSummary == null)
            {
                throw new Exception("Order Summary not found.");
            }
        }

        // Create the order and assign the summary
        var order = new Order
        {
            UserId = orderCreateDTO.UserId,
            OrderItems = orderItems,
            Status = OrderStatus.Pending, // Set initial status
            OrderSummaryId = orderSummary.OrderSummaryId // Assign the summary ID
        };

        var createdOrder = await _orderRepository.CreateOrderAsync(order);
        orderSummary.Orders.Add(createdOrder); // Add created order to the summary

        // Optionally, update the order summary in the repository if needed
        await _orderSummaryRepository.UpdateAsync(orderSummary);
        return new OrderResponseDTO
        {
            Id = createdOrder.Id,
            UserId = createdOrder.UserId,
            OrderItems = createdOrder.OrderItems.Select(oi => new OrderItemResponseDTO
            {
                MenuItemId = oi.MenuItemId,
                MenuItemName = oi.MenuItemName,
                Quantity = oi.Quantity,
                MenuItemPrice = oi.MenuItemPrice
            }).ToList(), // Map OrderItem to OrderItemResponseDTO
            TableNumber = orderSummary.TableNumber,
            OrderSummaryId = orderSummary.OrderSummaryId // Return the summary ID
        };
    }

    public async Task<OrderSummaryDto> GetOrderSummaryByIdAsync(Guid orderSummaryId)
    {
        var orderSummary = await _orderSummaryRepository.GetOrderSummaryByIdAsync(orderSummaryId);

        if (orderSummary == null)
        {
            throw new Exception("Order Summary not found.");
        }

        return new OrderSummaryDto
        {
            OrderSummaryId = orderSummary.OrderSummaryId,
            TableNumber = orderSummary.TableNumber,
            Orders = orderSummary.Orders.Select(o => new OrderResponseDTO // Ensure you're mapping to OrderResponseDTO
            {
                Id = o.Id,
                UserId = o.UserId,
                OrderItems = o.OrderItems.Select(oi => new OrderItemResponseDTO
                {
                    MenuItemId = oi.MenuItemId,
                    MenuItemName = oi.MenuItemName,
                    Quantity = oi.Quantity,
                    MenuItemPrice = oi.MenuItemPrice
                }).ToList(),
                Status = o.Status, // Ensure Status is included
                TotalPrice = o.TotalPrice // Include total price if needed
            }).ToList() // This should now be List<OrderResponseDTO>
        };
    }

    public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatusUpdateDTO orderStatusUpdateDTO)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        if (order == null)
        {
            throw new Exception("Order not found.");
        }

        // Update the order status
        order.Status = orderStatusUpdateDTO.NewStatus; // Assuming NewStatus is a property in OrderStatusUpdateDTO
        await _orderRepository.UpdateOrderAsync(order); // Make sure this method exists in your repository
    }

    public async Task CancelOrderAsync(Guid orderId)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        if (order == null)
        {
            throw new Exception("Order not found.");
        }

        await _orderRepository.CancelOrderAsync(orderId);
    }

    public async Task<List<OrderResponseDTO>> GetOrdersByUserIdAsync(Guid userId)
    {
        var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
        return orders.Select(o => new OrderResponseDTO
        {
            Id = o.Id,
            UserId = o.UserId,
            OrderItems = o.OrderItems.Select(oi => new OrderItemResponseDTO
            {
                MenuItemId = oi.MenuItemId,
                MenuItemName = oi.MenuItemName,
                Quantity = oi.Quantity,
                MenuItemPrice = oi.MenuItemPrice
            }).ToList(),
            Status = o.Status, // Ensure Status is set correctly
            TableNumber = o.OrderSummaryId.HasValue ? o.OrderSummary.TableNumber : 0 // Ensure TableNumber is correctly set
        }).ToList();
    }

    public async Task<OrderResponseDTO> GetOrderByIdAsync(Guid orderId)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        if (order == null)
        {
            throw new Exception("Order not found.");
        }

        return new OrderResponseDTO
        {
            Id = order.Id,
            UserId = order.UserId,
            OrderItems = order.OrderItems.Select(oi => new OrderItemResponseDTO
            {
                MenuItemId = oi.MenuItemId,
                MenuItemName = oi.MenuItemName,
                Quantity = oi.Quantity,
                MenuItemPrice = oi.MenuItemPrice
            }).ToList(),
            Status = order.Status,
            TableNumber = order.OrderSummaryId.HasValue ? order.OrderSummary.TableNumber : 0 // Set TableNumber correctly
        };
    }

    Task<OrderResponseDTO> IOrderService.UpdateOrderStatusAsync(Guid orderId, OrderStatusUpdateDTO orderStatusUpdateDTO)
    {
        throw new NotImplementedException();
    }

    Task<bool> IOrderService.CancelOrderAsync(Guid orderId)
    {
        throw new NotImplementedException();
    }
}
 