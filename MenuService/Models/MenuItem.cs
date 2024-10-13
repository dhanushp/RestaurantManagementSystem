using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuService.Models
{
    // MenuItem class represents the individual menu items
    public class MenuItem : BaseEntity
    {
        // Name of the menu item
        public string Name { get; set; }

        // Description of the menu item
        public string? Description { get; set; }

        // Price of the menu item, defined with decimal precision for accurate pricing
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Boolean indicating whether the item is non-vegetarian
        public bool IsNonVeg { get; set; }

        // Boolean indicating whether the item is availabe or not
        public bool IsAvailable { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        // Foreign key to Category
        public Guid CategoryId { get; set; }

        // Navigation property to the Category entity (relationship with Category model)
        public Category? Category { get; set; }
    }
}