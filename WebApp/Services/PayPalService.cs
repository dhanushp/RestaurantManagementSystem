using System.Net.Http.Json;
using Newtonsoft.Json;
using RestaurantManagement.SharedDataLibrary.DTOs.Payment;
using WebApp.DTOs;

namespace WebApp.Services
{
    public interface IPayPalService
    {
        Task<PayPalOrderResponseDTO> CreateOrder(PayPalCreateOrderDTO orderDto);
        Task<PayPalCaptureOrderResponseDTO> CaptureOrder(string orderId);
    }

    public class PayPalService : IPayPalService
    {
        private readonly HttpClient _httpClient;

        public PayPalService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PayPalOrderResponseDTO> CreateOrder(PayPalCreateOrderDTO orderDto)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:5004/api/checkout/create-order", orderDto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PayPalOrderResponseDTO>();
        }

        public async Task<PayPalCaptureOrderResponseDTO> CaptureOrder(string orderId)
        {
            var response = await _httpClient.PostAsync($"https://localhost:5004/api/checkout/capture-order/{orderId}", null);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PayPalCaptureOrderResponseDTO>();
        }
    }
}
