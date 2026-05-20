using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TaskManager.API.Hubs
{
    [Authorize]
    public class TaskHub : Hub
    {
        public async Task JoinSpace(string spaceId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"space-{spaceId}");
        }

        public async Task LeaveSpace(string spaceId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"space-{spaceId}");
        }
    }
}