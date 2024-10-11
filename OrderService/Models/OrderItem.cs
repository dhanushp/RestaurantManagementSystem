namespace OrderService.Models
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; } // Unique identifier for the order item
        public Guid MenuItemId { get; set; } // Reference to the menu item 
        public string? MenuItemName { get; set; } // Snapshot of the item name at the time of order
        public decimal MenuItemPrice { get; set; } // Snapshot of the item price at the time of order
        public int Quantity { get; set; } // Quantity of the item ordered
        public decimal TotalPrice => MenuItemPrice * Quantity; // Total price of the order item

        // Navigation properties
        public Guid OrderId { get; set; } // Foreign key linking to the Order
        public Order Order { get; set; } // Navigation property for the parent Order
    }
}
