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
using OrderService.Exceptions;
using Azure.Core;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;

public class OrderServiced : IOrderService
{
    //private IHttpClientFactory _httpClientFactory;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderSummaryRepository _orderSummaryRepository;

    private readonly HttpClient _httpClient;


    private readonly string _menuServiceUrl = "https://localhost:5003/api/menuitems"; // Change to actual URL
    private readonly string _userServiceUrl = "https://localhost:5003/api/users"; // Change to actual URL
    private readonly string _authenticationServiceUrl = "https://localhost:5003/api/authentication";
    // Constructor
    public OrderServiced(
        //IHttpClientFactory httpClientFactory,
        IOrderRepository orderRepository,
        IOrderSummaryRepository orderSummaryRepository,
        HttpClient httpClient)
    {
        //_httpClientFactory = httpClientFactory;
        _orderRepository = orderRepository;
        _orderSummaryRepository = orderSummaryRepository;
        _httpClient = httpClient;
    }

    public async Task<OrderResponseDTO> CreateOrderAsync(OrderCreateDTO orderCreateDTO)
    {
        try
        {
            // Fetch user details from UserService
            var admin_pwd = Environment.GetEnvironmentVariable("ADMIN_PWD");
            var loginData = new { email = "admin@eg.dk", password = admin_pwd };
            var json = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var loginResponse = await _httpClient.PostAsync(_authenticationServiceUrl + "/login", content);
            var responseBody = await loginResponse.Content.ReadAsStringAsync();
            var _loginResponse = JsonConvert.DeserializeObject<Response<LoginResponseDTO>>(responseBody);

            var accessToken = _loginResponse.Data.AccessToken;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Fetch user details
            var userResponse = await _httpClient.GetAsync($"{_userServiceUrl}/{orderCreateDTO.UserId}");
            if (!userResponse.IsSuccessStatusCode)
            {
                throw new Exception("User not found.");
            }

            var userDetails = await userResponse.Content.ReadFromJsonAsync<UserDTO>();

            // Fetch menu item details and create order items
            var orderItems = new List<OrderItem>();
            foreach (var item in orderCreateDTO.OrderItems)
            {
                var menuResponse = await _httpClient.GetAsync($"{_menuServiceUrl}/{item.MenuItemId}");
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
            var _orderSummaryId = Guid.NewGuid();
            if (orderCreateDTO.OrderSummaryId == null)
            {
                // Create a new order summary
                orderSummary = new OrderSummary
                {
                    Id = _orderSummaryId,
                    TableNumber = orderCreateDTO.TableNumber,
                    //Orders = new List<Order>() // Initialize list of orders
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
                OrderSummaryId = orderSummary.Id // Assign the summary ID
            };

            // Attempt to create the order and handle potential errors
            try
            {
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
                    OrderSummaryId = orderSummary.Id // Return the summary ID
                };
            }
            catch (DbUpdateException dbEx) // Catch database update exceptions
            {
                Console.WriteLine(dbEx.InnerException?.Message); // Log the inner exception
                throw new Exception("An error occurred while saving the order. See inner exception for details.", dbEx);
            }
        }
        catch (Exception ex)
        {
            // Log the exception details for further investigation
            Console.WriteLine($"Error creating order: {ex.Message}");
            throw; // Re-throw the exception to handle it upstream if necessary
        }
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
 