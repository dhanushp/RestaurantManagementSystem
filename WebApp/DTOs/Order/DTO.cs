using RestaurantManagement.SharedDataLibrary.Enums;

namespace WebApp.DTOs.Order
{
    public class CreateOrderRequestDTO
    {
        public Guid? OrderSummaryId { get; set; } // Null for first order
        public Guid TableId { get; set; }
        public string TableNumber { get; set; }
        public Guid UserId { get; set; }
        public string UserFullName { get; set; }
        public List<CreateOrderItemDTO> OrderItems { get; set; } = new List<CreateOrderItemDTO>();
    }

    public class CreateOrderItemDTO
    {
        public Guid MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public decimal MenuItemPrice { get; set; }
        public int Quantity { get; set; }

        public decimal? TotalPrice { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class OrderSummaryResponseDTO
    {
        public Guid OrderSummaryId { get; set; }
        public decimal SubTotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemResponseDTO> OrderItems { get; set; } = new List<OrderItemResponseDTO>();
    }

    public class OrderItemResponseDTO
    {
        public Guid OrderItemId { get; set; }
        public Guid MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public decimal MenuItemPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string ImageUrl { get; set; }
    }

    public class UpdateOrderItemStatusDTO
    {
        public Guid OrderItemId { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }

    public class OrderResponseDTO
    {
        public Guid OrderSummaryId { get; set; }
        public Guid OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemResponseDTO> OrderItems { get; set; } = new List<OrderItemResponseDTO>();
    }
}
