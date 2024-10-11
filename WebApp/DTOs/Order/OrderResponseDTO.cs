namespace WebApp.DTOs.Order
{
    public class OrderResponseDTO
    {
        public Guid OrderId { get; set; }               // The ID of the order
        public string TableNumber { get; set; }         // Table number associated with the order
        public List<OrderItemDto> OrderItems { get; set; }   // Items in the order
        public decimal TotalPrice { get; set; }         // Total price of the order
        public string Status { get; set; }              // Status of the order
        public DateTime OrderDate { get; set; }         // Date when the order was placed
    }
}
