using OrderService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.Interfaces;
using OrderService.Data;

namespace OrderService.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _dbContext; // Assuming you use Entity Framework

        public OrderRepository(OrderContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Create an order
        public async Task<Order> CreateOrderAsync(Order order)
        {
            _dbContext.Set<Order>().Add(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        // Get order by ID
        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _dbContext.Set<Order>()
                .Include(o => o.OrderItems) // Include OrderItems if necessary
                .Include(o => o.OrderSummary) // Include OrderSummary if needed
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        // Get all orders by user ID
        public async Task<List<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _dbContext.Set<Order>()
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        // Update an existing order
        public async Task UpdateOrderAsync(Order order)
        {
            _dbContext.Set<Order>().Update(order);
            await _dbContext.SaveChangesAsync();
        }

        // Cancel an order
        public async Task CancelOrderAsync(Guid orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order != null)
            {
                order.Status = OrderStatus.Cancelled; // Assuming you have an OrderStatus enum
                await UpdateOrderAsync(order); // Update the order
            }
        }

        // Update order status
        public async Task<Order> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                await UpdateOrderAsync(order); // Save changes
                return order; // Return the updated order
            }
            else
            {
                throw new Exception("Order not found.");
            }
        }
    }
}
