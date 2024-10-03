namespace AdminService.Models
{
    public class Order : BaseEntity
    {
        // Foreign key to User
        public int UserId { get; set; }
        // Navigation property to User
        public User User { get; set; }

        // List of OrderItems
        public List<OrderItem> OrderItems { get; set; }

        // Use enum for Status
        public OrderStatus Status { get; set; } // Enum type

        // Constructor to initialize the list
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}
