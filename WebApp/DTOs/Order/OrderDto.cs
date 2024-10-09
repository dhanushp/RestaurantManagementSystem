namespace WebApp.DTOs.Order
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public string TableNumber { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
