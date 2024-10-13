using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using PaymentService.Hubs;
using PaymentService.Interfaces;
using PaymentService.Models;
using RestaurantManagement.SharedDataLibrary.DTOs.Payment;
using RestaurantManagement.SharedDataLibrary.Enums;
using Serilog;

namespace PaymentService.Repositories
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IHubContext<PaymentHub> _hubContext;

        private string PayPalClientId { get; set; } = string.Empty;
        private string PayPalClientSecret { get; set; } = string.Empty;
        private string PayPalUrl { get; set; } = string.Empty;

        public CheckoutRepository(IConfiguration configuration, IPaymentRepository paymentRepository, IHubContext<PaymentHub> hubContext)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _paymentRepository = paymentRepository;
            _hubContext = hubContext;

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
            return accessTokenResponse.AccessToken ?? "";
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

            var payPalOrderResponse = JsonConvert.DeserializeObject<PayPalOrderResponseDTO>(responseBody);
            // Add the payment record to the PaymentRepository
            if(response.IsSuccessStatusCode)
            {
                var payment = new Payment
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    OrderId = payPalOrderResponse.Id,
                    Amount = createOrderDTO.Amount,
                    CurrencyCode = createOrderDTO.Currency,
                    PaymentMethod = PaymentMethod.PayPal,
                    Status = PaymentStatus.Pending
                };

                await _paymentRepository.AddPaymentAsync(payment);
            }
            
            return payPalOrderResponse;
        }

        public async Task<PayPalCaptureOrderResponseDTO> CaptureOrder(string orderId)
        {
            var accessToken = await GetPayPalAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var emptyContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(PayPalUrl + $"/v2/checkout/orders/{orderId}/capture", emptyContent);
            var responseBody = await response.Content.ReadAsStringAsync();

            var captureResponse = JsonConvert.DeserializeObject<PayPalCaptureOrderResponseDTO>(responseBody);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"Capture order failed: {response.StatusCode}, {responseBody}";
                Log.Error(errorMessage); // Use Serilog for logging the error
                throw new Exception(errorMessage);
            }

            // Check if the capture was successful
            if (captureResponse.Status == "COMPLETED")
            {
                // Find the payment record in the repository
                var payment = await _paymentRepository.GetPaymentByOrderIdAsync(orderId);

                if (payment != null)
                {
                    // Update the payment status to Success

                    payment.Status = PaymentStatus.Success;
                    payment.UpdatedAt = DateTime.Now;
                    payment.TransactionId = captureResponse.Id;
                    await _paymentRepository.UpdatePaymentAsync(payment);

                    // Notify the group about the payment status update
                    string groupName = $"{payment.UserId}-{payment.FoodOrderId}";
                    await _hubContext.Clients.Group(groupName)
                        .SendAsync("ReceivePaymentStatus", payment.FoodOrderId, payment.Status);
                }
            }

            return captureResponse;
        }

    }


}
