using WebApp.DTOs.Menu;
using WebApp.DTOs.Order;

namespace WebApp.Services
{
    public interface ICartService
    {
        void AddItem(MenuItemResponseDTO item);
        List<CreateOrderItemDTO> GetCurrentOrders();
        void ClearCart();
        int GetItemCount();
        decimal GetTotalPrice();
    }

    public class CartService : ICartService
    {
        private List<CreateOrderItemDTO> _currentOrders = new List<CreateOrderItemDTO>();

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
                _currentOrders.Add(new CreateOrderItemDTO
                {

                    MenuItemId = item.Id,
                    MenuItemName = item.Name,
                    MenuItemPrice = item.Price,
                    Quantity = 1,
                    TotalPrice = item.Price,
                    ImageUrl = item.ImageUrl
                });
            }
        }

        public List<CreateOrderItemDTO> GetCurrentOrders()
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
            return (decimal)_currentOrders.Sum(o => o.TotalPrice);
        }
    }
}
