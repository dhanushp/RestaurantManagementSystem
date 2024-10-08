using WebApp.DTOs.Users;

namespace WebApp.DTOs.SignIn
{
    public class JwtResponseDto
    {
        public UserResponseDTO UserInfo { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }

    }
}
