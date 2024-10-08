using OrderService.Models; // Use the namespace where the OrderStatus enum is defined

namespace OrderService.DTO
{
    public class OrderStatusUpdateDTO
    {
        public OrderStatus NewStatus { get; set; } // Reference the enum from Models
    }
}