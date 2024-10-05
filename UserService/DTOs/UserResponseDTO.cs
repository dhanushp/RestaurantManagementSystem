namespace UserService.DTOs
{
    public record UserResponseDTO(
        string FullName,
        string Email,
        string Role
    );
}

