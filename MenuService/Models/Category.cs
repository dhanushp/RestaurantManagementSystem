namespace MenuService.Models
{
    // Category class represents different categories of menu items
    public class Category : BaseEntity
    {
        // Name of the category (e.g., Appetizers, Main Course)
        public string Name { get; set; }

        // Optional description for the category
        public string Description { get; set; }

     
        // Navigation property for the one-to-many relationship with MenuItem
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>(); //Initialise to avoid nul reference
    }
}
