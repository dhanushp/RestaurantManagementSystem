using OrderService.Models;

namespace OrderService.Repositories
{
    public interface IOrderSummaryRepository
    {
        Task<OrderSummary?> GetOrderSummaryByIdAsync(Guid orderSummaryId); // Get order summary by ID
        Task AddAsync(OrderSummary orderSummary); // Add a new order summary
        Task UpdateAsync(OrderSummary orderSummary);
    }
}
