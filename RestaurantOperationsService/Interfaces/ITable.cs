using RestaurantManagement.SharedLibrary.Responses;
using RestaurantOperationsService.DTOs;

namespace RestaurantOperationsService.Interfaces
{
    public interface ITable
    {
        Task<Response<List<TableResponseDTO>>> GetAllTables();
        Task<Response<List<TableResponseDTO>>> GetAllAvailableTables();
        Task<Response<TableResponseDTO>> GetTableById(Guid tableId);
        Task<Response<TableResponseDTO>> CreateTable(TableCreateDTO tableCreateDTO);
        Task<Response<string>> UpdateTable(Guid tableId, TableUpdateDTO tableUpdateDTO);
        Task<Response<string>> SoftDeleteTable(Guid tableId);
        Task<Response<string>> MakeTableAvailable(Guid tableId);
        Task<Response<string>> OccupyTable(Guid tableId);
    }
}
