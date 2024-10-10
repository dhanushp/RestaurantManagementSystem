using Microsoft.JSInterop;
using Newtonsoft.Json;
using WebApp.DTOs;
using WebApp.DTOs.Tables;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApp.Services
{
    public interface ITableService
    {
        Task<Response<List<TableResponseDTO>>> GetAvailableTables();
        Task<Response<string>> OccupyTable(Guid tableId);
        Task<Response<TableResponseDTO>> GetTableByIdAsync(Guid tableId); // Fetch Table by ID
        Task<Guid?> GetSelectedTableFromLocalStorage();
        Task RemoveSelectedTableFromLocalStorage(); // Method to clear localStorage if needed
    }

    public class TableService : ITableService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly ITokenService _tokenService;

        public TableService(HttpClient httpClient, IJSRuntime jsRuntime, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _tokenService = tokenService;
        }

        // Fetch available tables
        public async Task<Response<List<TableResponseDTO>>> GetAvailableTables()
        {
            var token = await _tokenService.GetAccessToken();

            if (string.IsNullOrEmpty(token))
            {
                return Response<List<TableResponseDTO>>.ErrorResponse("Failed to retrieve access token", ErrorCode.ServiceUnavailable);
            }

            // Set the authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("https://localhost:5003/api/tables/available");
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<List<TableResponseDTO>>>(responseBody);
        }

        // Occupy a table and store its ID in local storage
        public async Task<Response<string>> OccupyTable(Guid tableId)
        {
            var token = await _tokenService.GetAccessToken();

            if (string.IsNullOrEmpty(token))
            {
                return Response<string>.ErrorResponse("Failed to retrieve access token", ErrorCode.ServiceUnavailable);
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new OccupyTableRequestDTO { TableId = tableId };
            var response = await _httpClient.PostAsJsonAsync("https://localhost:5003/api/tables/occupy", request);
            var responseBody = await response.Content.ReadAsStringAsync();

            var occupyResponse = JsonConvert.DeserializeObject<Response<string>>(responseBody);

            if (occupyResponse.Success)
            {
                // Store the tableId in local storage upon successful table selection
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "tableId", tableId.ToString());
            }

            return occupyResponse;
        }

        // Fetch table details by Id
        public async Task<Response<TableResponseDTO>> GetTableByIdAsync(Guid tableId)
        {
            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"https://localhost:5003/api/tables/{tableId}");
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<TableResponseDTO>>(responseBody);
        }

        // Retrieve selected table from local storage
        public async Task<Guid?> GetSelectedTableFromLocalStorage()
        {
            var tableIdStr = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "tableId");

            if (Guid.TryParse(tableIdStr, out var tableId))
            {
                return tableId;
            }

            return null;
        }

        // Method to clear the selected table from local storage (useful if the user wants to change the table)
        public async Task RemoveSelectedTableFromLocalStorage()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "tableId");
        }
    }
}
