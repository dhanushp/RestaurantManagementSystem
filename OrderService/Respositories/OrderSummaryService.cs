using OrderService.Repositories;
using OrderService.DTO;
using OrderService.Models;
using OrderService.Interfaces;
using OrderService.Exceptions;

namespace OrderService.Repositories
{
    public class OrderSummaryService : IOrderSummaryService
    {
        private readonly IOrderSummaryRepository _orderSummaryRepository;

        public OrderSummaryService(IOrderSummaryRepository orderSummaryRepository)
        {
            _orderSummaryRepository = orderSummaryRepository;
        }

        public async Task<OrderSummaryDto> GetOrderSummaryByIdAsync(Guid orderSummaryId)
        {
            var orderSummary = await _orderSummaryRepository.GetOrderSummaryByIdAsync(orderSummaryId);
            if (orderSummary == null) throw new NotFoundException("OrderSummary not found");

            return new OrderSummaryDto
            {
                Id = orderSummary.Id,
                TableNumber = orderSummary.TableNumber,
                TaxAmount = orderSummary.TaxAmount,
                Orders = orderSummary.Orders.Select(o => new OrderDto
                {
                    Id = o.Id,
                    Status = o.Status,
                    TotalPrice = o.TotalPrice,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemResponseDTO // Use OrderItemResponseDTO
                    {
                        MenuItemId = oi.MenuItemId,
                        MenuItemName = oi.MenuItemName,
                        Quantity = oi.Quantity,
                        MenuItemPrice = oi.MenuItemPrice, // Correct property instead of UnitPrice
                    }).ToList()
                }).ToList()
            };
        }


        public async Task CreateOrderSummaryAsync(CreateOrderSummaryDto createDto)
        {
            var newSummary = new OrderSummary
            {
                TableNumber = createDto.TableNumber,
                TaxAmount = createDto.TaxAmount,
                Orders = createDto.Orders.Select(o => new Order
                {
                    OrderItems = o.OrderItems.Select(oi => new OrderItem
                    {
                        MenuItemName = oi.MenuItemName,
                        Quantity = oi.Quantity,
                        MenuItemPrice = oi.MenuItemPrice
                    }).ToList(),
                    Status = OrderStatus.Pending // Default status
                }).ToList()
            };

            await _orderSummaryRepository.AddAsync(newSummary);
        }
    }
}
