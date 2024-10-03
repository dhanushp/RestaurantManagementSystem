using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewService.Models
{
    // This class stores reviews for individual menu items.
    public class Review : BaseEntity
    {
        public int OrderItemId { get; set; } // Reference to the specific item in the order
        public string ItemName { get; set; } // Snapshot of the item name at the time of the order

        [Column(TypeName = "decimal(2,1)")] // Rating scale 1.0 to 5.0
        public decimal Rating { get; set; } // Rating provided by the user for the item
        public string Feedback { get; set; } // User feedback (optional)
    }
}
