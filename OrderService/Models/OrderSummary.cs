namespace OrderService.Models
{
    public class OrderSummary : BaseEntity
    {
        public decimal TaxAmount { get; set; } // The tax amount applied to the orders in this summary
        public Guid OrderSummaryId { get; set; }
        public int TableNumber { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>(); // List to store multiple orders
        public Order Order { get; set; }
    }
}
