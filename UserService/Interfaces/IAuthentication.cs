using RestaurantManagement.SharedLibrary.Responses;
using UserService.DTOs;

namespace UserService.Interfaces
{
    public interface IAuthentication
    {
        Task<Response<string>> Register(UserRegisterDTO userDTO);

        Task<Response<JwtResponseDto>> Login(UserLoginDTO loginDTO);

        /*
        Task<Response<string>> ForgotPassword(UserRegisterDTO userDTO);

        Task<Response<string>> ResetPassword(UserRegisterDTO userDTO);

        */
    }
}
