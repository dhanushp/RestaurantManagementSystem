namespace OrderService.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; } // Unique identifier for the order item
        public int MenuItemId { get; set; } // Reference to the menu item
        public string MenuItemName { get; set; } // Snapshot of the item name
        public decimal MenuItemPrice { get; set; } // Snapshot of the item price
        public int Quantity { get; set; } // Quantity of the item ordered

        public int OrderId { get; set; } // Foreign key linking to Order
        public Order Order { get; set; } // Navigation property for the parent Order
    }
}
