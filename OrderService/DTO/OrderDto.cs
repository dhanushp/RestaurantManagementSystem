using OrderService.Models;
namespace OrderService.DTO
{
    public class OrderDto

    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }                         // Unique identifier for the order
        public OrderStatus Status { get; set; }              // Current status of the order (e.g., Pending, Completed)
        public decimal TotalPrice { get; set; }              // Total price of the order
        public List<OrderItemResponseDTO> OrderItems { get; set; }// List of items within this order

        //public int TableNumber { get; set; }
    }
}
