namespace RestaurantManagement.SharedDataLibrary.DTOs.User
{
    public record UserResponseDTO(
        Guid Id,
        string FullName,
        string Email,
        string Role,
        Guid RoleId
    );
}
