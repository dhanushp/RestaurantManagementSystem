namespace MenuService.DTOs
{
    public class MenuItemResponseDTO
    {
        public Guid Id { get; set; }             // Unique identifier for the menu item
        public string? Name { get; set; }          // Name of the menu item
        public string? Description { get; set; }   // Short description of the menu item
        public decimal Price { get; set; }        // Price of the menu item
        public string? Category { get; set; }   // Category the menu item belongs to
        public bool IsAvailable { get; set; }     // Indicates if the item is available for ordering
    }
}
