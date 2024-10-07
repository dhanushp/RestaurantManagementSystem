using RestaurantOperationsService.Models;

namespace RestaurantOperationsService.DTOs
{
    public record TableUpdateDTO
    {
        public string Number { get; set; }
        public int Capacity { get; set; }
        public TableStatus Status { get; set; }
    }
}
