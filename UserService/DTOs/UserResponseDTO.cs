namespace UserService.DTOs
{
    public record UserResponseDTO(
        Guid Id,
        string FullName,
        string Email,
        string Role
    );
}

