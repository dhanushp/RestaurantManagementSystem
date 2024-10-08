namespace OrderService.Models
{
    public class OrderSummary : BaseEntity
    {
        public int TableNumber { get; set; } // The table number associated with this order summary
        public decimal TaxAmount { get; set; } // The tax amount applied to the orders in this summary

        // Navigation property to reference the list of orders in this summary
        public List<Order> Orders { get; set; }

        // Constructor to initialize the Orders list
        public OrderSummary()
        {
            Orders = new List<Order>();
        }
    }
}
