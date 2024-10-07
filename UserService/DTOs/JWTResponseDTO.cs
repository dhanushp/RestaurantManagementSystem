namespace UserService.DTOs
{
    public class JwtResponseDto
    {
        public UserResponseDTO UserInfo { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }

    }
}
