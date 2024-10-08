using OrderService.Models;
using OrderService.Interfaces;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;

namespace OrderService.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _context;

        public OrderRepository(OrderContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId && o.DeletedAt == null) // Exclude deleted orders
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        public async Task<Order?> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order == null)
                return null;

            order.Status = newStatus;
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> CancelOrderAsync(Guid orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order == null)
                return false;

            order.DeletedAt = DateTime.UtcNow; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
