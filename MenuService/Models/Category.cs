namespace MenuService.Models
{
    // Category class represents different categories of menu items
    public class Category : BaseEntity
    {
        // Name of the category (e.g., Appetizers, Main Course)
        public string Name { get; set; }

        // Optional description for the category
        public string Description { get; set; }
    }
}
