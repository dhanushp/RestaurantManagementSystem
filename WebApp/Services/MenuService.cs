using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApp.DTOs;
using WebApp.DTOs.Menu;

namespace WebApp.Services
{
    // Interface IMenuService
    public interface IMenuService
    {
        Task<Response<List<CategoryDTO>>> GetCategoriesAsync();
        Task<Response<List<MenuItemResponseDTO>>> GetAllMenuItemsAsync();
        Task<Response<List<MenuItemResponseDTO>>> GetMenuItemsByCategoryAsync(Guid category);
        Task<Response<MenuItemDetailResponseDTO>> GetMenuItemByIdAsync(Guid id);
    }

    // Implementation of IMenuService
    public class MenuService : IMenuService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        // Constructor to inject HttpClient and TokenService
        public MenuService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        // Helper method to set Authorization header
        private async Task SetAuthorizationHeaderAsync()
        {
            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Method to fetch categories for display
        public async Task<Response<List<CategoryDTO>>> GetCategoriesAsync()
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync("https://localhost:5003/api/menuitems/categories");
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response<List<CategoryDTO>>>(responseBody);
        }

        // Method to fetch all menu items
        public async Task<Response<List<MenuItemResponseDTO>>> GetAllMenuItemsAsync()
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync("https://localhost:5003/api/menuitems/available");
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response<List<MenuItemResponseDTO>>>(responseBody);
        }

        // Method to fetch menu items by category
        public async Task<Response<List<MenuItemResponseDTO>>> GetMenuItemsByCategoryAsync(Guid category)
        {
            await SetAuthorizationHeaderAsync();
            var categoryIdJson = JsonConvert.SerializeObject(category);
            var content = new StringContent(categoryIdJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:5003/api/menuitems/category/", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response<List<MenuItemResponseDTO>>>(responseBody);
        }

        // Method to fetch a specific menu item by ID
        public async Task<Response<MenuItemDetailResponseDTO>> GetMenuItemByIdAsync(Guid id)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync($"https://localhost:5003/api/menuitems/{id}");
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response<MenuItemDetailResponseDTO>>(responseBody);
        }
    }
}
