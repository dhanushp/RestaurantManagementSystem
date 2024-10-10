namespace WebApp.DTOs.Menu
{
    public class MenuItemResponseDTO
    {
        public Guid Id { get; set; }           // Unique identifier for the menu item
        public string Name { get; set; }       // Name of the menu item
        public string Description { get; set; } // Description of the menu item
        public decimal Price { get; set; }     // Price of the menu item
        public string ImageUrl { get; set; }   // URL for the menu item's image
        public bool IsNonVeg { get; set; }     // Indicates if the item is non-vegetarian
        public string Category { get; set; }   // Category name (e.g., Soups, Veg Starters, etc.)
    }
}
