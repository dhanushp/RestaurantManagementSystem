using Microsoft.AspNetCore.SignalR;

namespace PaymentService.Hubs
{
    public class PaymentHub : Hub
    {
        // This method allows clients to register with a group based on their UserId and OrderSummaryId
        public async Task RegisterUser(Guid userId, Guid? orderSummaryId)
        {
            // Create a group name using the UserId and OrderSummaryId
            string groupName = $"{userId}-{orderSummaryId}";

            // Add the connection to the group
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        // This method can be used for when clients disconnect 
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Optional: Remove from groups or perform other cleanup actions if needed
            await base.OnDisconnectedAsync(exception);
        }
    }
}
