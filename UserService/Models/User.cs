namespace UserService.Models
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Password for authentication

        // Foreign key to Role
        public int RoleId { get; set; }
        public Role Role { get; set; } // Navigation property for the Role
    }
}
