namespace UserService.DTOs
{
    public class JwtResponseDto
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }

    }
}
