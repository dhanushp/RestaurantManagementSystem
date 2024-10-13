using System.Net.Http.Headers;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using RestaurantManagement.SharedDataLibrary.DTOs.User;
using WebApp.DTOs;

namespace WebApp.Services
{
    public interface IUserService
    {
        Task<Response<UserResponseDTO>> GetUserInfoAsync();
        Task<Guid> GetUserIdFromLocalStorage();
    }

    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly ITokenService _tokenService;

        public UserService(HttpClient httpClient, IJSRuntime jsRuntime, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _tokenService = tokenService;
        }

        public async Task<Response<UserResponseDTO>> GetUserInfoAsync()
        {
            var token = await _tokenService.GetAccessToken();

            if (string.IsNullOrEmpty(token))
            {
                return Response<UserResponseDTO>.ErrorResponse("Failed to retrieve access token", ErrorCode.ServiceUnavailable);
            }

            var userId = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userId");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("https://localhost:5003/api/users/" + userId);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<UserResponseDTO>>(responseBody);

        }

        public async Task<Guid> GetUserIdFromLocalStorage()
        {
            return await _jsRuntime.InvokeAsync<Guid>("localStorage.getItem", "userId");
        }
    }

}
