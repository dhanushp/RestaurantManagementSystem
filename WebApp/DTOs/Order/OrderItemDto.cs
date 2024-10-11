namespace WebApp.DTOs.Order
{
    public class OrderItemDto
    {
        public Guid MenuItemId { get; set; }
        public string Name { get; set; } // Assuming 'Name' refers to the menu item
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } // Order status (e.g., Pending, Ready)
    }
}
