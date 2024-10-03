using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentService.Models
{
    public class Payment : BaseEntity
    {
        public int OrderId { get; set; } // Foreign key reference to the order

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } // Total payment amount

        public string PaymentMethod { get; set; } // Payment method (Cash, Credit Card, etc.)

        public string TransactionId { get; set; } // Online transaction ID

        public PaymentStatus Status { get; set; } // Payment status (enum)
    }

    // Enum for predefined payment statuses
    public enum PaymentStatus
    {
        Pending,
        Success,
        Failed
    }
}
