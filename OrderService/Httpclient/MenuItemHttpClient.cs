using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MenuService.DTOs; // Assuming you have a MenuItemDetailResponseDTO defined
using RestaurantManagement.SharedLibrary.Responses;

namespace OrderAPI.Infrastructure.HttpClients
{
    public class MenuItemHttpClient
    {
        private readonly HttpClient _httpClient;

        public MenuItemHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Fetch menu item details based on menu item ID
        public async Task<Response<MenuItemDetailResponseDTO>> GetMenuItemDetailsByIdAsync(Guid menuItemId)
        {
            var response = await _httpClient.GetAsync($"/api/menuitems/{menuItemId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Response<MenuItemDetailResponseDTO>>();
            }
            else
            {
                return new Response<MenuItemDetailResponseDTO>
                {
                    Success = false,
                    Message = "Failed to fetch menu item details",
                    Data = null
                };
            }
        }
    }
}
