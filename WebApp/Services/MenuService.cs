using System.Net.Http.Json;
using WebApp.DTOs.Menu;

namespace WebApp.Services
{
    public interface IMenuService
    {
        Task<List<MenuItemResponseDTO>> GetMenuItemsAsync();
        Task<List<CategoryDTO>> GetCategoriesAsync();
    }

    public class MenuService : IMenuService
    {
        private readonly HttpClient _httpClient;

        public MenuService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<MenuItemResponseDTO>> GetMenuItemsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<MenuItemResponseDTO>>("https://localhost:5003/api/menu");
            return response ?? new List<MenuItemResponseDTO>();
        }

        public async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CategoryDTO>>("https://localhost:5003/api/menu/categories");
            return response ?? new List<CategoryDTO>();
        }
    }
}
