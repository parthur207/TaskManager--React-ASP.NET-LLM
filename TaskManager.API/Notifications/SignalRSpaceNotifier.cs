using Microsoft.AspNetCore.SignalR;
using TaskManager.API.Hubs;
using TaskManager.Core.DTOs;
using TaskManager.Core.Ports.Notifications;

namespace TaskManager.API.Notifications
{
    public class SignalRSpaceNotifier : ISpaceNotifier
    {
        private readonly IHubContext<TaskHub, ITaskHubClient> _hub;

        public SignalRSpaceNotifier(IHubContext<TaskHub, ITaskHubClient> hub)
            => _hub = hub;

        public Task NotifyTaskCreated(Guid spaceId, TaskDTO task)
            => _hub.Clients.Group($"space-{spaceId}").TaskCreated(task);

        public Task NotifyTaskUpdated(Guid spaceId, TaskDTO task)
            => _hub.Clients.Group($"space-{spaceId}").TaskUpdated(task);

        public Task NotifyTaskDeleted(Guid spaceId, Guid taskId)
            => _hub.Clients.Group($"space-{spaceId}").TaskDeleted(taskId);

        public Task NotifySpaceUpdated(Guid spaceId)
            => _hub.Clients.Group($"space-{spaceId}").SpaceUpdated(spaceId);
    }
}
