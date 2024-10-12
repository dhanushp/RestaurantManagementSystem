using Microsoft.EntityFrameworkCore;
using RestaurantManagement.SharedLibrary.Responses;
using RestaurantOperationsService.Data;
using RestaurantOperationsService.DTOs;
using RestaurantOperationsService.Interfaces;
using RestaurantOperationsService.Models;
using RestaurantManagement.SharedLibrary.Data;

namespace RestaurantOperationsService.Repositories
{
    public class TableRepository : ITable
    {
        private readonly RestaurantOperationsDbContext _context;

        public TableRepository(RestaurantOperationsDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<TableResponseDTO>>> GetAllTables()
        {
            var roles = await _context.Tables
                .Where(r => r.DeletedAt == null) // Exclude soft-deleted roles
                .ToListAsync();

            var tables = await _context.Tables.Select(t => new TableResponseDTO
            {
                Id = t.Id,
                Number = t.Number,
                Capacity = t.Capacity,
                Status = t.Status,
            }).ToListAsync();

            return Response<List<TableResponseDTO>>.SuccessResponse("Fetched Tables Successfully", tables);
        }

        public async Task<Response<List<TableResponseDTO>>> GetAllAvailableTables()
        {
            var tables = await _context.Tables
                .Where(t => t.DeletedAt == null)        // Exclude soft-deleted tables
                .Where(t => t.Status == TableStatus.Available) // Filter only available tables
                .Select(t => new TableResponseDTO
                {
                    Id = t.Id,
                    Number = t.Number,
                    Capacity = t.Capacity,
                    Status = t.Status,
                })
                .ToListAsync();

            return Response<List<TableResponseDTO>>.SuccessResponse("Fetched Available Tables Successfully", tables);
        }


        public async Task<Response<TableResponseDTO>> GetTableById(Guid tableId)
        {
            var table = await _context.Tables.FindAsync(tableId);
            if (table == null) return Response<TableResponseDTO>.ErrorResponse("Table not found",ErrorCode.UserNotFound);

            var tableDTO = new TableResponseDTO
            {
                Id = table.Id,
                Number = table.Number,
                Capacity = table.Capacity,
                Status = table.Status
            };

            return Response<TableResponseDTO>.SuccessResponse("Fetched Table Successfully",tableDTO);
        }

        public async Task<Response<TableResponseDTO>> CreateTable(TableCreateDTO tableCreateDTO)
        {
            var table = new Table
            {
                Number = tableCreateDTO.Number,
                Capacity = tableCreateDTO.Capacity,
                Status = tableCreateDTO.Status,
                CreatedAt = DateTime.Now
            };

            _context.Tables.Add(table);
            await _context.SaveChangesAsync();

            var tableDTO = new TableResponseDTO
            {
                Id = table.Id,
                Number = table.Number,
                Capacity = table.Capacity,
                Status = table.Status
            };

            return Response<TableResponseDTO>.SuccessResponse("Table Created Successfully",tableDTO);
        }

        public async Task<Response<string>> UpdateTable(Guid tableId, TableUpdateDTO tableUpdateDTO)
        {
            var table = await _context.Tables.FindAsync(tableId);
            if (table == null) return Response<string>.ErrorResponse("Table not found", ErrorCode.TableNotFound);

            table.Number = tableUpdateDTO.Number;
            table.Capacity = tableUpdateDTO.Capacity;
            table.Status = tableUpdateDTO.Status;
            table.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Response<string>.SuccessResponse("Table updated successfully", $"{ tableId}");
        }

        public async Task<Response<string>> SoftDeleteTable(Guid tableId)
        {
            var table = await _context.Tables.FindAsync(tableId);
            if (table == null) return Response<string>.ErrorResponse("Table not found", ErrorCode.TableNotFound);

            table.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Response<string>.SuccessResponse("Table deleted successfully", $"{tableId}");
        }

        public async Task<Response<string>> MakeTableAvailable(Guid tableId)
        {
            var table = await _context.Tables.FindAsync(tableId);
            if (table == null) return Response<string>.ErrorResponse("Table not found", ErrorCode.TableNotFound);

            table.Status = TableStatus.Available; // Make the table available
            await _context.SaveChangesAsync();

            return Response<string>.SuccessResponse("Table is now available", $"{tableId}");
        }


        public async Task<Response<string>> OccupyTable(Guid tableId)
        {
            var table = await _context.Tables.FindAsync(tableId);
            if (table == null || table.Status != TableStatus.Available)
            {
                return Response<string>.ErrorResponse("Table not found or already occupied", ErrorCode.TableNotFound);
            }

            table.Status = TableStatus.Occupied;
            table.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Response<string>.SuccessResponse("Table occupied successfully", $"{tableId}");
        }
        
    }
}
