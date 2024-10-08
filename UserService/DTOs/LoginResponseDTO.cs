namespace UserService.DTOs
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; }

        public DateTime AccessTokenExpiresAt { get; set; }

        public string RefreshToken { get; set; } 

        public DateTime RefreshTokenExpiresAt { get; set; }

        public UserResponseDTO UserInfo { get; set; }

    }
}
