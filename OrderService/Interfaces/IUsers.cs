using UserService.DTOs;
using RestaurantManagement.SharedLibrary.Responses;

namespace OrderService.Interfaces
{
    public interface IUsers
    {
        Task<Response<UserDetailResponseDTO>> GetUserDetailsByIdAsync(Guid userId);
    }
}
