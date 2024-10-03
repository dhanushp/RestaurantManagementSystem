using System.ComponentModel.DataAnnotations;

namespace UserService.DTOs
{
    public record UserDTO(
        [Required] string FullName,
        [Required, EmailAddress] string Email,
        [Required] string PasswordHash,
        [Required] string PasswordSalt,
        [Required] string Role
    );
}
