namespace RestaurantManagement.SharedDataLibrary.DTOs.Payment
{
    public class PayPalCreateOrderDTO
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
    }

}
