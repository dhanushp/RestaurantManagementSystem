using Newtonsoft.Json;

namespace RestaurantManagement.SharedDataLibrary.DTOs.Payment
{
    public class PayPalOrderResponseDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("links")]
        public List<PayPalLinkDescriptionDTO> Links { get; set; }
    }

    public class PayPalLinkDescriptionDTO
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("rel")]
        public string Rel { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }
    }

}
