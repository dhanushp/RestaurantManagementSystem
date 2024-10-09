namespace WebApp.DTOs.Order
{
    public class OrderItemDto
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
