namespace WebApp.DTOs.Order
{
    public class OrderSummaryDto
    {
        public Guid OrderId { get; set; }               // Unique identifier for the order
        public string TableNumber { get; set; }         // The table number
        public decimal TotalPrice { get; set; }         // Total price of the order
        public string Status { get; set; }              // Current status of the order
        public DateTime OrderDate { get; set; }         // Date when the order was placed
    }
}
