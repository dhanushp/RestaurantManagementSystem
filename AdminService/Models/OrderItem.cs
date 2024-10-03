using System.ComponentModel.DataAnnotations.Schema;

namespace AdminService.Models
{
    public class OrderItem
    {
        /*
         By adding OrderId and the navigation property, we establish a relationship between OrderItem and Order.
         This allows EF Core to recognize the foreign key and create the appropriate database relationship.       
         */
        public int OrderItemId { get; set; }

        // Foreign key to Order
        public int OrderId { get; set; }
        // Navigation property to Order
        public Order Order { get; set; }

        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MenuItemPrice { get; set; }

        public int Quantity { get; set; }
    }
}
