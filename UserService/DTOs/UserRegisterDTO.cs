using System.ComponentModel.DataAnnotations;

namespace UserService.DTOs
{
    public record UserRegisterDTO(
        [Required] string FullName,
        [Required] string Email,
        [Required] string Password
    );
}
