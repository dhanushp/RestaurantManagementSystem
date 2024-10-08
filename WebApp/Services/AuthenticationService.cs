using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using WebApp.DTOs;
using WebApp.DTOs.SignIn;
using WebApp.DTOs.SignUp;
using static WebApp.Pages.SignUp;
using Microsoft.JSInterop;


namespace WebApp.Services
{
    public interface IAuthenticationService
    {
        Task<Response<JwtResponseDto>> Login(LoginModel loginModel);
        Task<Response<string>> Register(SignupModel signupModel); 

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

        public async Task<Response<JwtResponseDto>> Login(LoginModel loginModel)
        {
            var json = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:5003/api/authentication/login", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            var loginResponse = JsonConvert.DeserializeObject<Response<JwtResponseDto>>(responseBody);

            // Store token in local storage if login was successful
            if (loginResponse.Success && loginResponse.Data != null)
            {
                var token = loginResponse.Data.Token;
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "jwtToken", token);
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
    }
}
