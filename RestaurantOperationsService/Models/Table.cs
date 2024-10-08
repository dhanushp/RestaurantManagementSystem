using RestaurantManagement.SharedLibrary.Models;

namespace RestaurantOperationsService.Models
{
    public class Table : BaseEntity
    {
        public string Number { get; set; } // Table number
        public int Capacity { get; set; }
        public TableStatus Status { get; set; }

    }

    public enum TableStatus
    {
        Occupied,
        Available,
        Reserved
    }

}
