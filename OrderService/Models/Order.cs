namespace OrderService.Models
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; } // Reference to the user who placed the order
        public List<OrderItem> OrderItems { get; set; } // List of items in the order

        public OrderStatus Status { get; set; } // Enum for order status (Pending, InPreparation, Served, Paid)
    }

    // Enum for predefined order statuses
    public enum OrderStatus
    {
        Pending,
        InPreparation,
        Served,
        Paid // Paid status for marking completed payments
    }
}
