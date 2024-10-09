using OrderService.DTO;

namespace OrderService.Interfaces
{
    public interface IOrderSummaryService
    {
        Task<OrderSummaryDto> GetOrderSummaryByIdAsync(Guid orderSummaryId); // Get order summary by ID
        Task CreateOrderSummaryAsync(CreateOrderSummaryDto createDto); // Create a new order summary
    }
}
