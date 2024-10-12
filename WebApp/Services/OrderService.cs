using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApp.DTOs;
using WebApp.DTOs.Order;
using Microsoft.JSInterop;
namespace WebApp.Services
{
    // Interface declaration
    public interface IOrderService
    {
        Task<Response<List<OrderDto>>> GetAllOrdersAsync();
        Task<Response<OrderDto>> GetOrderByIdAsync(Guid orderId);
        Task<Response<List<OrderDto>>> GetOrdersByUserIdAsync(Guid userId);
        Task<Response<string>> UpdateOrderStatusAsync(Guid orderId, OrderStatusUpdateDTO statusUpdate);
        Task<Response<string>> CancelOrderAsync(Guid orderId);
        Task<Response<OrderResponseDTO>> PlaceOrderAsync(CreateOrderRequestDTO orderCreateDTO);
        // Declaration of PlaceOrderAsync
        Task StoreOrderSummaryIdInLocalStorage(Guid orderSummaryID);
    }

    // Class implementation
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly ITokenService _tokenService;

        public OrderService(HttpClient httpClient, IJSRuntime jsRuntime, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _tokenService = tokenService;
        }

        public async Task<Response<List<OrderDto>>> GetAllOrdersAsync()
        {
            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("http://localhost:5181/api/orders");
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<List<OrderDto>>>(responseBody);
        }

        public async Task<Response<OrderDto>> GetOrderByIdAsync(Guid orderId)
        {
            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"http://localhost:5181/api/orders/{orderId}");
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<OrderDto>>(responseBody);
        }

        public async Task<Response<List<OrderDto>>> GetOrdersByUserIdAsync(Guid userId)
        {
            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"http://localhost:5181/api/orders/user/{userId}");
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<List<OrderDto>>>(responseBody);
        }

        public async Task<Response<string>> UpdateOrderStatusAsync(Guid orderId, OrderStatusUpdateDTO statusUpdate)
        {
            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync($"http://localhost:5181/api/orders/{orderId}/status", statusUpdate);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<string>>(responseBody);
        }

        public async Task<Response<string>> CancelOrderAsync(Guid orderId)
        {
            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"http://localhost:5181/api/orders/{orderId}");
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<string>>(responseBody);
        }

        // Implementation of PlaceOrderAsync
        public async Task<Response<OrderResponseDTO>> PlaceOrderAsync(CreateOrderRequestDTO orderCreateDTO)
        {
            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("https://localhost:5003/api/orders/create", orderCreateDTO);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<OrderResponseDTO>>(responseBody);
        }

        public async Task StoreOrderSummaryIdInLocalStorage(Guid orderSummaryID)
        {
            // Store the orderSummaryID in local storage

            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "orderSummaryID", orderSummaryID.ToString());
        }
    }
}
