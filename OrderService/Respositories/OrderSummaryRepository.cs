using OrderService.Data;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using OrderService.Repositories;
using OrderService.Interfaces;

namespace OrderService.Repositories
{
    public class OrderSummaryRepository : IOrderSummaryRepository
    {
        private readonly OrderContext _context;

        public OrderSummaryRepository(OrderContext context)
        {
            _context = context;
        }

        public async Task<OrderSummary?> GetOrderSummaryByIdAsync(Guid orderSummaryId)
        {
            return await _context.OrderSummaries
                .Include(os => os.Orders)
                .ThenInclude(o => o.OrderItems)
                .FirstOrDefaultAsync(os => os.Id == orderSummaryId);
        }

        public async Task AddAsync(OrderSummary orderSummary)
        {
            await _context.OrderSummaries.AddAsync(orderSummary);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrderSummary orderSummary)
        {
            // Check if the entity is already tracked. If not, attach it.
            var existingOrderSummary = await _context.OrderSummaries
                .Include(os => os.Orders) // Ensure that Orders are included
                .FirstOrDefaultAsync(os => os.Id == orderSummary.Id);

            if (existingOrderSummary != null)
            {
                // Update properties of the existing entity
                _context.Entry(existingOrderSummary).CurrentValues.SetValues(orderSummary);

                // Update related orders if necessary
                foreach (var order in orderSummary.Orders)
                {
                    var existingOrder = existingOrderSummary.Orders.FirstOrDefault(o => o.Id == order.Id);
                    if (existingOrder != null)
                    {
                        _context.Entry(existingOrder).CurrentValues.SetValues(order);
                        // Similarly, you can update the OrderItems if needed
                    }
                    else
                    {
                        existingOrderSummary.Orders.Add(order); // Add new orders
                    }
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
