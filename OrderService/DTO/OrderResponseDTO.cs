namespace OrderService.DTOs
{
    public class OrderResponseDTO
    {
        public int Id { get; set; } // Unique identifier of the order
        public int UserId { get; set; } // Reference to the user who placed the order
        public List<OrderItemResponseDTO> OrderItems { get; set; } // List of items in the order
        public string Status { get; set; } // Current order status (Pending, InPreparation, Served, Paid)
        public decimal TotalPrice { get; set; } // Total price of the order
        public DateTime CreatedAt { get; set; } // Order creation time
        public DateTime? UpdatedAt { get; set; } // Last update time

        // Ensure the list is initialized to avoid null reference issues
        public OrderResponseDTO()
        {
            OrderItems = new List<OrderItemResponseDTO>();
        }
    }

    public class OrderItemResponseDTO
    {
        public int MenuItemId { get; set; } // Reference to the menu item
        public string MenuItemName { get; set; } // Snapshot of the item name at the time of order
        public decimal MenuItemPrice { get; set; } // Snapshot of the item price at the time of order
        public int Quantity { get; set; } // Quantity of the item ordered
        public decimal TotalPrice => MenuItemPrice * Quantity; // Total price of the order item
    }
}
