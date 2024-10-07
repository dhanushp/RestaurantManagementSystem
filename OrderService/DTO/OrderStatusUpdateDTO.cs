namespace OrderService.DTOs
{
    public class OrderStatusUpdateDTO
    {
        public OrderStatus NewStatus { get; set; } // The new status to be applied to the order (Pending, InPreparation, Served, Paid)
    }

    // You can reuse the OrderStatus enum from the models:
    public enum OrderStatus
    {
        Pending,
        InPreparation,
        Served,
        Paid
    }
}
