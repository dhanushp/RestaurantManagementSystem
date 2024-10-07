using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using RestaurantManagement.SharedLibrary.Responses;
using UserService.DTOs; // Assuming you have a UserDetailResponseDTO defined

namespace OrderAPI.Infrastructure.HttpClients
{
    public class UserHttpClient
    {
        private readonly HttpClient _httpClient;

        public UserHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Fetch user details based on user ID
        public async Task<Response<UserDetailResponseDTO>> GetUserDetailsByIdAsync(Guid userId)
        {
            var response = await _httpClient.GetAsync($"/api/users/{userId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Response<UserDetailResponseDTO>>();
            }
            else
            {
                return new Response<UserDetailResponseDTO>
                {
                    Success = false,
                    Message = "Failed to fetch user details",
                    Data = null
                };
            }
        }
    }
}
