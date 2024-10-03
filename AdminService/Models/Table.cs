namespace AdminService.Models
{
    public class Table : BaseEntity
    {
        public int Number { get; set; } // Table number
        public bool IsOccupied { get; set; } // Whether the table is occupied or not
    }
}
