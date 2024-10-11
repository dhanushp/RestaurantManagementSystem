using RestaurantManagement.SharedLibrary.Models;
using RestaurantManagement.SharedDataLibrary.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    public class OrderItem : BaseEntity
    {
        public Guid MenuItemId { get; set; } // Reference to the menu item
        public string? MenuItemName { get; set; } // Snapshot of the item name at the time of order
        [Column(TypeName = "decimal(10, 2)")]
        public decimal MenuItemPrice { get; set; } // Snapshot of the item price at the time of order
        public int Quantity { get; set; } // Quantity of the item ordered

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPrice => MenuItemPrice * Quantity; // Total price of the order item
        public OrderStatus OrderStatus { get; set; } // Status of the order item

        // Foreign key and navigation property for Order
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
