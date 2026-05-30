using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TaskManager.Core.Ports.Notifications;

namespace TaskManager.API.Hubs
{
    [Authorize]
    public class TaskHub : Hub<ITaskHubClient>
    {
        public async Task JoinSpace(string spaceId)
            => await Groups.AddToGroupAsync(Context.ConnectionId, $"space-{spaceId}");

        public async Task LeaveSpace(string spaceId)
            => await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"space-{spaceId}");
    }
}