namespace OrderService.DTO
{
    public class OrderSummaryDto
    {
        public Guid Id { get; set; }
        public int TableNumber { get; set; }

        public Guid? OrderSummaryId { get; set; }
        public decimal TaxAmount { get; set; }
        public List<OrderDto> Orders { get; set; }
    }
}
