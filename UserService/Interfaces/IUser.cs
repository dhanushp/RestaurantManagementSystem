using RestaurantManagement.SharedLibrary.Responses;
using UserService.DTOs;

namespace UserService.Interfaces
{
    public interface IUser
    {
        Task<Response<List<UserResponseDTO>>> GetAllUsers();
        Task<Response<List<UserResponseDTO>>> GetUsersByRole(Guid roleId);
        Task<Response<UserResponseDTO>> GetUserByEmail(string email);
        Task<Response<UserResponseDTO>> GetUserById(Guid userId);
        Task<Response<string>> UpdateUserRole(Guid userId, Guid roleId);
        Task<Response<string>> UpdateUserFullName(Guid userId, string fullName);
        Task<Response<string>> SoftDeleteUser(Guid userId);
    }
}
