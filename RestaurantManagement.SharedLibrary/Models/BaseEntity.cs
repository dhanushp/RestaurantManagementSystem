using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.SharedLibrary.Models
{
    public abstract class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } // Primary key

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Creation timestamp

        public DateTime? UpdatedAt { get; set; } // Optional update timestamp

        public DateTime? DeletedAt { get; set; } // Optional soft deletion timestamp
    }
}
