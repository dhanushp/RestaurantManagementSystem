namespace AdminService.Models
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // Admin, Staff, Customer

        // Navigation property to Orders
        public List<Order> Orders { get; set; }

        // Constructor to initialize the list
        public User()
        {
            Orders = new List<Order>();
        }
    }
}
