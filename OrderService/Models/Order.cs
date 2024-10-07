using OrderService.Models;

namespace OrderService.Models
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; } // Reference to the user who placed the order
        public List<OrderItem> OrderItems { get; set; } // List of items in the order
        public OrderStatus Status { get; set; } // Current status of the order

        // Computed property for total order price
        public decimal TotalPrice => OrderItems?.Sum(item => item.TotalPrice) ?? 0;

        // Constructor ensures the list is initialized to avoid null references
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}
