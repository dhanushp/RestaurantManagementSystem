namespace WebApp.DTOs.Menu
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }            // Unique identifier for the category
        public string Name { get; set; }       // Name of the category

        public string? Description { get; set; }
    }
}
