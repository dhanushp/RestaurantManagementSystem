using System.ComponentModel.DataAnnotations.Schema;
using RestaurantManagement.SharedLibrary.Models;
using RestaurantManagement.SharedDataLibrary.Enums;

namespace PaymentService.Models
{
    public class Payment : BaseEntity
    {
        public string OrderId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } // Total payment amount

        public string CurrencyCode { get; set; }

        public PaymentMethod PaymentMethod { get; set; } // Cash, PayPal, Credit/Debit Card

        public string? TransactionId { get; set; } // Online transaction ID

        public PaymentStatus Status { get; set; } // Payment status (enum)

        public string? UserId { get; set; }  
        public int? FoodOrderId { get; set; } 

    }

}
