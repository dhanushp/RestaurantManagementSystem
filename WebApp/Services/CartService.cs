using WebApp.DTOs.Menu;
using WebApp.DTOs.Order;
using System.Linq;

namespace WebApp.Services
{
    public interface ICartService
    {
        void AddItem(MenuItemResponseDTO item);
        List<CreateOrderItemDTO> GetCurrentOrders();
        void RemoveItem(Guid menuItemId); // Change to Guid
        void DecreaseQuantity(Guid menuItemId); // Change to Guid
        void ClearCart();
        int GetItemCount();
        decimal GetTotalPrice();
    }

    public class CartService : ICartService
    {
        private List<CreateOrderItemDTO> _currentOrders = new List<CreateOrderItemDTO>();

        public void AddItem(MenuItemResponseDTO item)
        {
            var existingOrder = _currentOrders.FirstOrDefault(o => o.MenuItemId == item.Id); // Change comparison to Guid
            if (existingOrder != null)
            {
                existingOrder.Quantity++;
                existingOrder.TotalPrice += item.Price;
            }
            else
            {
                _currentOrders.Add(new CreateOrderItemDTO
                {
                    MenuItemId = item.Id, // Ensure this is Guid
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

        public void RemoveItem(Guid menuItemId) // Change parameter to Guid
        {
            var orderToRemove = _currentOrders.FirstOrDefault(o => o.MenuItemId == menuItemId);
            if (orderToRemove != null)
            {
                _currentOrders.Remove(orderToRemove);
            }
        }

        public void DecreaseQuantity(Guid menuItemId) // Change parameter to Guid
        {
            var existingOrder = _currentOrders.FirstOrDefault(o => o.MenuItemId == menuItemId);
            if (existingOrder != null)
            {
                existingOrder.Quantity--;
                existingOrder.TotalPrice -= existingOrder.MenuItemPrice;

                if (existingOrder.Quantity <= 0)
                {
                    _currentOrders.Remove(existingOrder);
                }
            }
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
