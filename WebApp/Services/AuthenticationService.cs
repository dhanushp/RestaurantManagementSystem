using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using WebApp.DTOs;
using WebApp.DTOs.SignIn;
using WebApp.DTOs.SignUp;
using Microsoft.JSInterop;


namespace WebApp.Services
{
    public interface IAuthenticationService
    {
        Task<Response<LoginResponseDTO>> Login(LoginModel loginModel);
        Task<Response<string>> Register(SignupModel signupModel);
        Task<Response<LoginResponseDTO>> RefreshAccessToken(); 

    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;


        public AuthenticationService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task<Response<LoginResponseDTO>> Login(LoginModel loginModel)
        {
            var json = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:5003/api/authentication/login", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            var loginResponse = JsonConvert.DeserializeObject<Response<LoginResponseDTO>>(responseBody);

            // Store tokens in local storage if login was successful
            if (loginResponse.Success && loginResponse.Data != null)
            {
                var token = loginResponse.Data.AccessToken;
                var refreshToken = loginResponse.Data.RefreshToken; // Get refresh token
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "jwtToken", token);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "refreshToken", refreshToken); // Store refresh token
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "accessTokenExpiresAt", loginResponse.Data.AccessTokenExpiresAt.ToString());
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userId", loginResponse.Data.UserInfo.Id);

            }

            return loginResponse;
        }

        public async Task<Response<string>> Register(SignupModel signupModel)
        {
            var json = JsonConvert.SerializeObject(signupModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:5003/api/authentication/register", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<string>>(responseBody); // Deserializing response to handle errors or success message
        }

        public async Task<Response<LoginResponseDTO>> RefreshAccessToken()
        {
            var refreshToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "refreshToken");

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Response<LoginResponseDTO>.ErrorResponse("No refresh token found", ErrorCode.InvalidCredentials);
            }

            var json = JsonConvert.SerializeObject(new { RefreshToken = refreshToken });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:5003/api/authentication/refreshAccessToken", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            var refreshResponse = JsonConvert.DeserializeObject<Response<LoginResponseDTO>>(responseBody);

            // Store new access token and refresh token if refresh was successful
            if (refreshResponse.Success && refreshResponse.Data != null)
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "jwtToken", refreshResponse.Data.AccessToken);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "refreshToken", refreshResponse.Data.RefreshToken);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "accessTokenExpiresAt", refreshResponse.Data.AccessTokenExpiresAt.ToString());

            }

            return refreshResponse;
        }

    }
}
