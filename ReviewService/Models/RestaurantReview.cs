using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewService.Models
{
    // This class stores overall reviews for the restaurant experience.
    public class RestaurantReview : BaseEntity
    {
        public int UserId { get; set; } // Reference to the User
        public string UserName { get; set; } // Snapshot of the user's name

        [Column(TypeName = "decimal(2,1)")] // Rating scale 1.0 to 5.0
        public decimal Rating { get; set; } // Overall rating for the restaurant experience
        public string Feedback { get; set; } // Optional feedback from the user
    }
}
