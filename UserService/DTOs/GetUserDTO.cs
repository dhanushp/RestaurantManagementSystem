namespace UserService.DTOs
{
    public record GetUserDTO(
        string FullName,
        string Email,
        string Role
    );
}

