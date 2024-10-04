using System.ComponentModel.DataAnnotations;

namespace UserService.DTOs
{
    public record UserLoginDto(
        [Required] string Email,
        [Required] string Password
    );

}
