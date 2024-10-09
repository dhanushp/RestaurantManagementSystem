namespace WebApp.DTOs.Cart
{
    public class CartSummaryDto
    {
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public decimal TotalPrice { get; set; }
        public int TotalItems { get; set; }
    }
}
