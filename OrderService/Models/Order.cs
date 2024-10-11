using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OrderService.Models;
using RestaurantManagement.SharedLibrary.Models;

namespace OrderService.Models
{
    public class Order : BaseEntity
    {
        public List<OrderItem> OrderItems { get; set; } // List of items in the order
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Guid? OrderSummaryId { get; set; }
        // Navigation property for the parent OrderSummary
        [NotMapped]
        public OrderSummary OrderSummary { get; set; }

        // Computed property for total order price
        public decimal TotalPrice => OrderItems?.Sum(item => item.TotalPrice) ?? 0;

        // Constructor ensures the list is initialized to avoid null references
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}
