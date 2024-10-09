using System.Net.Http.Json;
using WebApp.DTOs;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using WebApp.DTOs.Menu;

namespace WebApp.Services
{
    public interface IMenuService
    {
        Task<Response<List<MenuItemResponseDTO>>> GetAllMenuItemsAsync();
        Task<Response<List<MenuItemResponseDTO>>> GetAvailableMenuItemsAsync();
        Task<Response<List<MenuItemResponseDTO>>> GetMenuItemsByCategoryAsync(string category);
        Task<Response<MenuItemDetailResponseDTO>> GetMenuItemByIdAsync(Guid id);
        Task<Response<MenuItemDetailResponseDTO>> GetMenuItemByNameAsync(string name);
    }

    public class MenuService : IMenuService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public MenuService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        public async Task<Response<List<MenuItemResponseDTO>>> GetAllMenuItemsAsync()
        {
            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("http://localhost:5001/api/menuitems");
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<List<MenuItemResponseDTO>>>(responseBody);
        }

        public async Task<Response<List<MenuItemResponseDTO>>> GetAvailableMenuItemsAsync()
        {
            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("http://localhost:5001/api/menuitems/available");
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<List<MenuItemResponseDTO>>>(responseBody);
        }

        public async Task<Response<List<MenuItemResponseDTO>>> GetMenuItemsByCategoryAsync(string category)
        {
            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"http://localhost:5001/api/menuitems/category/{category}");
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<List<MenuItemResponseDTO>>>(responseBody);
        }

        public async Task<Response<MenuItemDetailResponseDTO>> GetMenuItemByIdAsync(Guid id)
        {
            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"http://localhost:5001/api/menuitems/{id}");
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<MenuItemDetailResponseDTO>>(responseBody);
        }

        public async Task<Response<MenuItemDetailResponseDTO>> GetMenuItemByNameAsync(string name)
        {
            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"http://localhost:5001/api/menuitems/name/{name}");
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<MenuItemDetailResponseDTO>>(responseBody);
        }
    }
}
