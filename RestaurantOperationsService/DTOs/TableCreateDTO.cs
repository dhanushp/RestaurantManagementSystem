using RestaurantOperationsService.Models;

namespace RestaurantOperationsService.DTOs
{
    public record TableCreateDTO
    {
        public string Number { get; set; }
        public int Capacity { get; set; }
        public TableStatus Status { get; set; } = TableStatus.Available; // Default status when created
    }
}
