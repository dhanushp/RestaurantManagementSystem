using UserService.DTOs;

namespace OrderService.DTO
{
    public class OrderCreateDTO
    {
        public Guid UserId { get; set; } // Reference to the user who placed the order
        public List<OrderItemCreateDTO> OrderItems { get; set; } // List of items being ordered

        public Guid? OrderSummaryId { get; set; }
        public int TableNumber { get; set; }

        // Ensure the list is initialized to avoid null reference issues
        public OrderCreateDTO()
        {
            OrderItems = new List<OrderItemCreateDTO>();
        }
    }

    public class OrderItemCreateDTO
    {
        public Guid MenuItemId { get; set; } // Reference to the menu item
        public string MenuItemName { get; set; }
        public decimal MenuItemPrice { get; set; }
        public int Quantity { get; set; } // Quantity of the item being ordered
    }
}
