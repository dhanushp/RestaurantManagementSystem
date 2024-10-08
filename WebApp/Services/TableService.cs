using Microsoft.JSInterop;
using Newtonsoft.Json;
using WebApp.DTOs;
using WebApp.DTOs.Tables;
using System.Net.Http.Headers;
using static WebApp.Pages.SelectTable;
using System.Net.Http.Json;

namespace WebApp.Services
{
    public interface ITableService
    {
        Task<Response<List<TableResponseDTO>>> GetAvailableTables();

        Task<Response<string>> OccupyTable(Guid tableId); 
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

        public async Task<Response<String>> OccupyTable(Guid tableId)
        {
            var token = await _tokenService.GetAccessToken();

            if (string.IsNullOrEmpty(token))
            {
                return Response<String>.ErrorResponse("Failed to retrieve access token", ErrorCode.ServiceUnavailable);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new OccupyTableRequestDTO { TableId = tableId };
            var response = await _httpClient.PostAsJsonAsync("https://localhost:5003/api/tables/occupy", request);

            var responseBody = await response.Content.ReadAsStringAsync();

            var occupyResponse = JsonConvert.DeserializeObject<Response<string>>(responseBody);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "tableId", tableId);
            return occupyResponse;
        }
    }
}
