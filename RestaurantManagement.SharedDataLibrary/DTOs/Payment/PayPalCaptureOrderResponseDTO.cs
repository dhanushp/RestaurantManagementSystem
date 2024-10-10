using Newtonsoft.Json;

namespace RestaurantManagement.SharedDataLibrary.DTOs.Payment
{
    public class PayPalCaptureOrderResponseDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
