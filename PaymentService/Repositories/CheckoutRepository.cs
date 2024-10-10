using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using PaymentService.Interfaces;
using RestaurantManagement.SharedDataLibrary.DTOs.Payment;

namespace PaymentService.Repositories
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        private string PayPalClientId { get; set; } = string.Empty;
        private string PayPalClientSecret { get; set; } = string.Empty;
        private string PayPalUrl { get; set; } = string.Empty;

        public CheckoutRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<string> GetPayPalAccessToken()
        {
            PayPalClientId = _configuration["PayPalSettings:ClientId"];
            PayPalClientSecret = Environment.GetEnvironmentVariable("PAYPAL_SECRET");
            if (string.IsNullOrEmpty(PayPalClientSecret))
            {
                throw new InvalidOperationException("Environment variable 'PAYPAL_SECRET' is not set.");
            }
            PayPalUrl = _configuration["PayPalSettings:Url"];

            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{PayPalClientId}:{PayPalClientSecret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await _httpClient.PostAsync(PayPalUrl + "/v1/oauth2/token", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            var accessTokenResponse = JsonConvert.DeserializeObject<PayPalAccessTokenResponse>(responseBody);
            return accessTokenResponse.AccessToken;
        }

        public async Task<PayPalOrderResponseDTO> CreateOrder(PayPalCreateOrderDTO createOrderDTO)
        {
            var accessToken = await GetPayPalAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var orderRequest = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                new
                {
                    amount = new { value = createOrderDTO.Amount.ToString("F2"), currency_code = createOrderDTO.Currency }
                }
            }
            };

            var content = new StringContent(JsonConvert.SerializeObject(orderRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(PayPalUrl + "/v2/checkout/orders", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PayPalOrderResponseDTO>(responseBody);
        }

        public async Task<PayPalCaptureOrderResponseDTO> CaptureOrder(string orderId)
        {
            var accessToken = await GetPayPalAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.PostAsync($"https://api-m.sandbox.paypal.com/v2/checkout/orders/{orderId}/capture", null);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PayPalCaptureOrderResponseDTO>(responseBody);
        }
    }


}
