using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Models
{
    public abstract class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } // primary key
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Creation timestamp
        public DateTime? UpdatedAt { get; set; } // Timestamp when the entity is updated
        public DateTime? DeletedAt { get; set; } // Soft deletion timestamp
    }
}
