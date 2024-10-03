using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } // Password for authentication
        public string PasswordSalt { get; set; } // Add this field for the salt


        // Foreign key to Role
        public int RoleId { get; set; }
        public Role Role { get; set; } // Navigation property for the Role
    }
}
