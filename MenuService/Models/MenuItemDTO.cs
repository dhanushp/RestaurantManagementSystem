namespace MenuService.Models
{
    public class MenuItemDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsNonVeg { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageUrl { get; set; }

        public string Category { get; set; } 
    }
}
