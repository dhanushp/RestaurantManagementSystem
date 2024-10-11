namespace WebApp.DTOs.Order
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }              // Unique identifier for the order
        public Guid UserId { get; set; }               // The ID of the user who placed the order
        public string TableNumber { get; set; }        // The table number associated with the order
        public List<OrderItemDto> OrderItems { get; set; }   // List of items in the order
        public decimal TotalPrice { get; set; }        // Total price of the order
        public string Status { get; set; }             // Current status of the order (e.g., Pending, Completed)
        public DateTime OrderDate { get; set; }        // Date when the order was placed
    }
}
