using Microsoft.AspNetCore.SignalR;
using TaskManager.Core.DTOs;
using TaskManager.Core.Ports.Notifications;
using TaskManager.API.Hubs;

namespace TaskManager.Adapters.Notifications
{
    public class TaskNotificationAdapter : ITaskNotificationPort
    {
        private readonly IHubContext<TaskHub> _hubContext;

        public TaskNotificationAdapter(IHubContext<TaskHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyTaskCreated(Guid spaceId, TaskDTO task)
        {
            await _hubContext.Clients
                .Group($"space-{spaceId}")
                .SendAsync("TaskCreated", task);
        }

        public async Task NotifyTaskUpdated(Guid spaceId, TaskDTO task)
        {
            await _hubContext.Clients
                .Group($"space-{spaceId}")
                .SendAsync("TaskUpdated", task);
        }

        public async Task NotifyTaskDeleted(Guid spaceId, Guid taskId)
        {
            await _hubContext.Clients
                .Group($"space-{spaceId}")
                .SendAsync("TaskDeleted", taskId);
        }
    }
}