using Microsoft.JSInterop;

namespace WebApp.Services
{
    public interface ITokenService
    {
        Task<string?> GetAccessToken();
    }

    public class TokenService : ITokenService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IAuthenticationService _authenticationService;

        public TokenService(IJSRuntime jsRuntime, IAuthenticationService authenticationService)
        {
            _jsRuntime = jsRuntime;
            _authenticationService = authenticationService;
        }

        public async Task<string?> GetAccessToken()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwtToken");

            // Check if the token is expired
            var tokenExpiryString = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "accessTokenExpiresAt");
            if (DateTime.TryParse(tokenExpiryString, out var tokenExpiry) && DateTime.UtcNow >= tokenExpiry)
            {
                var refreshResponse = await _authenticationService.RefreshAccessToken();
                if (refreshResponse.Success)
                {
                    return refreshResponse.Data.AccessToken;
                }
                else
                {
                    await Logout(); // Log out user if refresh fails
                    return null;
                }
            }

            return token;
        }

        public async Task Logout()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "jwtToken");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "accessTokenExpiresAt");
            
        }
    }

}
