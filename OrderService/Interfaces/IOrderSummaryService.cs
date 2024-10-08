using OrderService.DTO;

namespace OrderService.Interfaces
{
    public interface IOrderSummaryService
    {
        Task<OrderSummaryDto> GetOrderSummaryByIdAsync(Guid orderSummaryId);
        Task CreateOrderSummaryAsync(CreateOrderSummaryDto createDto);
        // Additional methods like updating, deleting summaries can be added here.
    }
}
