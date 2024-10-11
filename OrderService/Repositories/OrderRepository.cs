using RestaurantManagement.SharedDataLibrary.DTOs.Order;
using RestaurantManagement.SharedLibrary.Responses;
using OrderService.Data;
using OrderService.Models;
using RestaurantManagement.SharedDataLibrary.Enums;
using Microsoft.EntityFrameworkCore;
using OrderService.Interfaces;

namespace OrderService.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<OrderResponseDTO>> CreateOrderAsync(CreateOrderRequestDTO orderDto)
        {
            // Check if order summary exists
            OrderSummary orderSummary = null;
            if (orderDto.OrderSummaryId.HasValue)
            {
                orderSummary = await _context.OrderSummaries.FindAsync(orderDto.OrderSummaryId.Value);
                if (orderSummary == null)
                    return Response<OrderResponseDTO>.ErrorResponse("Order summary not found.");
            }
            else
            {
                // Create a new order summary if not provided
                orderSummary = new OrderSummary
                {
                    SubTotalAmount = 0,
                    TaxAmount = 0,
                    TotalAmount = 0,
                    TableId = orderDto.TableId,
                    TableNumber = orderDto.TableNumber,
                    UserId = orderDto.UserId,
                    UserFullName = orderDto.UserFullName
                };
                _context.OrderSummaries.Add(orderSummary);
                await _context.SaveChangesAsync(); // Save to generate ID
            }

            decimal subtotal = orderDto.OrderItems.Sum(item => item.MenuItemPrice * item.Quantity);
            decimal taxAmount = (10m / 100) * subtotal; 
            decimal totalAmount = subtotal + taxAmount;

            var order = new Order
            {
                OrderSummaryId = orderSummary.Id,
                TotalAmount = orderDto.OrderItems.Sum(item => item.MenuItemPrice * item.Quantity),
                OrderItems = orderDto.OrderItems.Select(item => new OrderItem
                {
                    MenuItemId = item.MenuItemId,
                    MenuItemName = item.MenuItemName,
                    MenuItemPrice = item.MenuItemPrice,
                    Quantity = item.Quantity,
                    TotalPrice = item.MenuItemPrice * item.Quantity,
                    OrderStatus = OrderStatus.Pending,
                    ImageUrl = item.ImageUrl,
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Updating the order summary with the calculated amounts
            orderSummary.SubTotalAmount += subtotal;
            orderSummary.TaxAmount += taxAmount;
            orderSummary.TotalAmount += totalAmount;

            _context.OrderSummaries.Update(orderSummary);
            await _context.SaveChangesAsync();

            return Response<OrderResponseDTO>.SuccessResponse("Order created successfully.", new OrderResponseDTO
            {
                OrderSummaryId = orderSummary.Id,
                OrderId = order.Id,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(oi => new OrderItemResponseDTO
                {
                    OrderItemId = oi.Id,
                    MenuItemId = oi.MenuItemId,
                    MenuItemName = oi.MenuItemName,
                    MenuItemPrice = oi.MenuItemPrice,
                    Quantity = oi.Quantity,
                    TotalPrice = oi.TotalPrice,
                    OrderStatus = oi.OrderStatus,
                    ImageUrl = oi.ImageUrl
                }).ToList()
            });
        }

        public async Task<Response<OrderSummaryResponseDTO>> GetOrderSummaryByIdAsync(Guid orderSummaryId)
        {
            var orderSummary = await _context.OrderSummaries
                .Include(o => o.Orders)
                .ThenInclude(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderSummaryId);

            if (orderSummary == null)
                return Response<OrderSummaryResponseDTO>.ErrorResponse("Order summary not found.");

            var orderItems = orderSummary.Orders.SelectMany(o => o.OrderItems)
                .GroupBy(oi => new {oi.Id, oi.MenuItemId, oi.MenuItemName, oi.MenuItemPrice, oi.OrderStatus, oi.ImageUrl })
                .Select(g => new OrderItemResponseDTO
                {
                    OrderItemId = g.Key.Id,
                    MenuItemId = g.Key.MenuItemId,
                    MenuItemName = g.Key.MenuItemName,
                    MenuItemPrice = g.Key.MenuItemPrice,
                    Quantity = g.Sum(i => i.Quantity),
                    TotalPrice = g.Sum(i => i.TotalPrice),
                    OrderStatus = g.Key.OrderStatus,
                    ImageUrl = g.Key.ImageUrl
                })
                .ToList();

            var orderSummaryDto = new OrderSummaryResponseDTO
            {
                OrderSummaryId = orderSummary.Id,
                SubTotalAmount = orderSummary.SubTotalAmount,
                TaxAmount = orderSummary.TaxAmount,
                TotalAmount = orderSummary.TotalAmount,
                OrderItems = orderItems
            };

            return Response<OrderSummaryResponseDTO>.SuccessResponse("Order summary retrieved successfully.", orderSummaryDto);
        }

        public async Task<Response<List<OrderResponseDTO>>> GetOrdersByUserIdAsync(Guid userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.OrderSummary.UserId == userId)
                .ToListAsync();

            if (orders == null || !orders.Any())
                return Response<List<OrderResponseDTO>>.ErrorResponse("No orders found for this user.");

            var orderDtos = orders.Select(o => new OrderResponseDTO
            {
                OrderSummaryId = o.OrderSummaryId,
                OrderId = o.Id,
                TotalAmount = o.TotalAmount,
                OrderItems = o.OrderItems.Select(oi => new OrderItemResponseDTO
                {
                    OrderItemId = oi.Id,
                    MenuItemId = oi.MenuItemId,
                    MenuItemName = oi.MenuItemName,
                    MenuItemPrice = oi.MenuItemPrice,
                    Quantity = oi.Quantity,
                    TotalPrice = oi.TotalPrice,
                    OrderStatus = oi.OrderStatus,
                    ImageUrl = oi.ImageUrl
                }).ToList()
            }).ToList();

            return Response<List<OrderResponseDTO>>.SuccessResponse("Orders retrieved successfully.", orderDtos);
        }

        public async Task<Response<bool>> UpdateOrderItemStatusAsync(UpdateOrderItemStatusDTO updateOrderItemStatusDto)
        {
            var orderItem = await _context.OrderItems.FindAsync(updateOrderItemStatusDto.OrderItemId);

            if (orderItem == null)
                return Response<bool>.ErrorResponse("Order item not found.");

            orderItem.OrderStatus = updateOrderItemStatusDto.OrderStatus;
            await _context.SaveChangesAsync();

            return Response<bool>.SuccessResponse($"Order item status updated to {OrderStatusExtensions.OrderStatusName[updateOrderItemStatusDto.OrderStatus]} successfully.", true);
        }
    }
}
