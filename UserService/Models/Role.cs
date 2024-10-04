using RestaurantManagement.SharedLibrary.Models;

namespace UserService.Models
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } // Name of the role (e.g., admin, staff, customer)

        public string? Description { get; set; } // Optional description of the role

        public virtual ICollection<User>? Users { get; set; } // Navigation property for user
    }
}
