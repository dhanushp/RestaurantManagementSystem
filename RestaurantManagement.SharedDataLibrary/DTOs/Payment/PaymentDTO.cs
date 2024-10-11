using RestaurantManagement.SharedDataLibrary.Enums;

namespace RestaurantManagement.SharedDataLibrary.DTOs.Payment
{
    public class PaymentDTO
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
    }

    public class UpdatePaymentStatusDTO
    {
        public PaymentStatus Status { get; set; }
    }
}

