using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class Role : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } // Name of the role (e.g., admin, staff, customer)

        public string? Description { get; set; } // Optional description of the role

        public virtual ICollection<User>? Users { get; set; } // Navigation property for user
    }
}
