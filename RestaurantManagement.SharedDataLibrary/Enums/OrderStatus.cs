namespace RestaurantManagement.SharedDataLibrary.Enums
{
    public enum OrderStatus
    {
        Pending,
        InPreparation,
        ReadyForPickup,
        Served,
        Paid,
        Cancelled
    }

    public static class OrderStatusExtensions
    {
        // Mapping OrderStatus to a string description.
        public static readonly Dictionary<OrderStatus, string> OrderStatusName = new Dictionary<OrderStatus, string>
        {
            { OrderStatus.Pending, "Pending" },
            { OrderStatus.InPreparation, "In Preparation" },
            { OrderStatus.ReadyForPickup, "Ready for Pickup" },
            { OrderStatus.Served, "Served" },
            { OrderStatus.Paid, "Paid" },
            { OrderStatus.Cancelled, "Cancelled" }
        };

        public static string GetOrderStatusName(this OrderStatus status)
        {
            return OrderStatusName.ContainsKey(status) ? OrderStatusName[status] : "Unknown";
        }
    }
}
