using System.ComponentModel.DataAnnotations.Schema;
using RestaurantManagement.SharedLibrary.Models;

namespace OrderService.Models
{
    public class Order : BaseEntity
    {
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalAmount { get; set; }

        // Foreign key and navigation property for OrderSummary
        public Guid OrderSummaryId { get; set; }
        public OrderSummary OrderSummary { get; set; }

        // Navigation property for related OrderItems
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
