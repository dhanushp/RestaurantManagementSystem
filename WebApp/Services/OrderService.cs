using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.JSInterop;
using WebApp.DTOs;
using WebApp.DTOs.Order;

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
        Task<Response<OrderResponseDTO>> PlaceOrderAsync(CreateOrderRequestDTO orderCreateDTO); // Declaration of PlaceOrderAsync

        Task StoreOrderSummaryIdInLocalStorage(Guid? orderSummaryID);
        Task<Guid?> GetOrderSummaryIdFromLocalStorage();

        

       // Task<List<CreateOrderItemDTO>> GetOrdersFromLocalStorage();

        //Task StoreOrdersInLocalStorage(List<CreateOrderItemDTO> orders);
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

        public async Task StoreOrderSummaryIdInLocalStorage(Guid? orderSummaryID)
        {
            // Store the orderSummaryID in local storage

            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "orderSummaryID", orderSummaryID.ToString());
        }

        public async Task<Guid?> GetOrderSummaryIdFromLocalStorage()
        {
            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // Attempt to get the OrderSummaryId from local storage
            var orderSummaryIDString = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "orderSummaryID");

            // If OrderSummaryId is found, return it as a Guid, otherwise return null
            if (!string.IsNullOrEmpty(orderSummaryIDString) && Guid.TryParse(orderSummaryIDString, out Guid orderSummaryID))
            {
                return orderSummaryID;
            }

            return null;
        }



        /*public async Task<List<CreateOrderItemDTO>> GetOrdersFromLocalStorage()
        {

            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // Retrieve the orders JSON string from local storage
            var ordersJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "Orders");

            // Deserialize and return the list of orders, or return an empty list if null
            return string.IsNullOrEmpty(ordersJson)
                ? new List<CreateOrderItemDTO>()
                : JsonConvert.DeserializeObject<List<CreateOrderItemDTO>>(ordersJson);
        }
*/
       /* public async Task StoreOrdersInLocalStorage(List<CreateOrderItemDTO> orders)
        {

            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // Serialize the orders and store them in local storage
            var ordersJson = JsonConvert.SerializeObject(orders);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "Orders", ordersJson);
        }*/
    }
}
