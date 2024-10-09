namespace WebApp.DTOs.Order
{
    public class OrderCreateDTO
    {
        public List<OrderItemDto> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
        public string TableNumber { get; set; }
    }
}
