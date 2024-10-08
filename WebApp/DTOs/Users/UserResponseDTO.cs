namespace WebApp.DTOs.Users
{
    public record UserResponseDTO(
        Guid Id,
        string FullName,
        string Email,
        string Role,
        Guid RoleId
    );
}
