using System.ComponentModel.DataAnnotations.Schema;
using RestaurantManagement.SharedLibrary.Models;

namespace OrderService.Models
{
    public class OrderSummary : BaseEntity
    {
        [Column(TypeName = "decimal(18, 2)")]
        public decimal SubTotalAmount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TaxAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        // Table and User information
        public Guid TableId { get; set; }
        public string TableNumber { get; set; }
        public Guid UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;

        // Navigation property for related Orders
        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
