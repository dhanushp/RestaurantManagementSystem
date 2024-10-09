using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text;
using WebApp.DTOs.Order;

namespace WebApp.Services
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetUserOrdersAsync();
        Task<bool> PlaceOrder(OrderCreateDTO order);
        Task<bool> UpdateOrderStatus(int orderId, OrderStatusUpdateDTO statusUpdate);
    }

    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<OrderDto>> GetUserOrdersAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<OrderDto>>("https://localhost:5003/api/orders");
            return response ?? new List<OrderDto>();
        }

        public async Task<bool> PlaceOrder(OrderCreateDTO order)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:5003/api/orders", order);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateOrderStatus(int orderId, OrderStatusUpdateDTO statusUpdate)
        {
            var content = new StringContent(JsonConvert.SerializeObject(statusUpdate), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"https://localhost:5003/api/orders/{orderId}/status", content);
            return response.IsSuccessStatusCode;
        }
    }
}
