using OrderService.Models;

namespace OrderService.Repositories
{
    public interface IOrderSummaryRepository
    {
        Task<OrderSummary?> GetOrderSummaryByIdAsync(Guid orderSummaryId);
        Task AddAsync(OrderSummary orderSummary);
    }
}
