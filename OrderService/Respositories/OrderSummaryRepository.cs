using OrderService.Data;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using OrderService.Repositories;

namespace OrderAPI.Infrastructure.Repositories
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
    }
}
