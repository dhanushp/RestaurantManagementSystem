using OrderService.Models;
using RestaurantManagement.SharedDataLibrary.Enums;

namespace OrderService.DTO
{
    public class OrderResponseDTO
    {
        public Guid Id { get; set; } // Unique identifier of the order
        public Guid UserId { get; set; } // Reference to the user who placed the order
        public int TableNumber { get; set; }
        public List<OrderItemResponseDTO> OrderItems { get; set; } // List of items in the order

        public Guid? OrderSummaryId { get; set; }
        public OrderStatus Status { get; set; }
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
        public Guid MenuItemId { get; set; } // Reference to the menu item
        public string MenuItemName { get; set; } // Snapshot of the item name at the time of order
        public decimal MenuItemPrice { get; set; } // Snapshot of the item price at the time of order
        public int Quantity { get; set; } // Quantity of the item ordered
        public decimal TotalPrice => MenuItemPrice * Quantity; // Total price of the order item
    }
}
