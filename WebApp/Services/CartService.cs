using WebApp.DTOs.Menu;
using WebApp.DTOs.Order;

namespace WebApp.Services
{
    public interface ICartService
    {
        void AddItem(MenuItemResponseDTO item);
        List<OrderItemDto> GetCurrentOrders();
        void ClearCart();
        int GetItemCount();
        decimal GetTotalPrice();
    }

    public class CartService : ICartService
    {
        private List<OrderItemDto> _currentOrders = new List<OrderItemDto>();

        public void AddItem(MenuItemResponseDTO item)
        {
            var existingOrder = _currentOrders.FirstOrDefault(o => o.MenuItemId == item.Id);
            if (existingOrder != null)
            {
                existingOrder.Quantity++;
                existingOrder.TotalPrice += item.Price;
            }
            else
            {
                _currentOrders.Add(new OrderItemDto
                {
                    MenuItemId = item.Id,
                    Name = item.Name,
                    Quantity = 1,
                    TotalPrice = item.Price
                });
            }
        }

        public List<OrderItemDto> GetCurrentOrders()
        {
            return _currentOrders;
        }

        public void ClearCart()
        {
            _currentOrders.Clear();
        }

        public int GetItemCount()
        {
            return _currentOrders.Sum(o => o.Quantity);
        }

        public decimal GetTotalPrice()
        {
            return _currentOrders.Sum(o => o.TotalPrice);
        }
    }
}
