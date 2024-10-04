using RestaurantManagement.SharedLibrary.Responses;
using UserService.DTOs;

namespace UserService.Interfaces
{
    public interface IUser
    {
        Task<Response> Register(UserDTO userDTO);

        Task<Response> Login(UserLoginDto loginDTO);

        Task<GetUserDTO> GetUser(Guid userId);
    }
}
