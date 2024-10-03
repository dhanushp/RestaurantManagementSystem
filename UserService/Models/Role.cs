namespace UserService.Models
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } // Name of the role (e.g., admin, staff, customer)
        public string Description { get; set; } // Optional description of the role
    }
}
