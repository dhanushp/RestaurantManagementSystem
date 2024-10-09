namespace MenuService.DTOs
{
    public class MenuItemDetailResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        // Add other relevant properties as needed
        public string? Category { get; set; }
    }
}
