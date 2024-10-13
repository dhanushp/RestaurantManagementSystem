using System.ComponentModel.DataAnnotations.Schema;
using RestaurantManagement.SharedDataLibrary.Enums;

namespace RestaurantManagement.SharedDataLibrary.DTOs.Payment
{
    public class PaymentCashDTO
    {
        public Guid? UserId { get; set; }
        public Guid? FoodOrderId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class UpdatePaymentStatusDTO
    {
        public PaymentStatus Status { get; set; }
    }
}

