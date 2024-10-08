namespace OrderService.DTO
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }                         // Unique identifier for the order item
        public string Name { get; set; }                     // Name of the item
        public int Quantity { get; set; }                    // Quantity of the item ordered
        public decimal UnitPrice { get; set; }               // Price per unit of the item
        public decimal TotalPrice => Quantity * UnitPrice;   // Total price for this item (Quantity * UnitPrice)
    }
}
