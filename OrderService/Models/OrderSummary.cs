using RestaurantManagement.SharedLibrary.Models;

namespace OrderService.Models
{
    public class OrderSummary : BaseEntity
    {
        public decimal TaxAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public Guid TableId { get; set; } // Reference to table Id placed
        public int TableNumber { get; set; }
        public Guid UserId { get; set; } // Reference to the user who placed the order

        public List<Order> Orders { get; set; } = new List<Order>(); // List to store multiple orders
        public Order Order { get; set; }
    }
}
