namespace OrderService.DTO
{
    public class CreateOrderSummaryDto
    {
        public int TableNumber { get; set; }
        public decimal TaxAmount { get; set; }
        public List<OrderCreateDTO> Orders { get; set; }
    }
}
