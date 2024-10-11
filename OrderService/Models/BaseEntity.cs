namespace OrderService.Models
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } // Common primary key
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Creation timestamp
        public DateTime? UpdatedAt { get; set; } // Timestamp when the entity is updated
        public DateTime? DeletedAt { get; set; } // Soft deletion timestamp (optional)
    }
}
