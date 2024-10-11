namespace WebApp.DTOs.Order
{
    public class OrderCreateDTO
    {
        public Guid UserId { get; set; }           // The ID of the user placing the order
        public string TableNumber { get; set; }    // The table number
        public List<OrderItemDto> OrderItems { get; set; }    // List of items being ordered
        public decimal TotalPrice { get; set; }    // Total price of the order
    }
}
