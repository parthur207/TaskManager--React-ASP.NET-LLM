using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;
using TaskManager.API.Hubs.RealTime;
using TaskManager.Core.DTOs;
using TaskManager.Core.Ports.Notifications;

namespace TaskManager.API.Realtime.Notifiers
{
    public sealed class TaskNotifier : ITaskNotifier
    {
        private readonly IHubContext<TaskManagerHub, ITaskManagerHubClient> _hub;
        private readonly ILogger<TaskNotifier> _logger;

        public TaskNotifier(IHubContext<TaskManagerHub, ITaskManagerHubClient> hub, ILogger<TaskNotifier> logger)
        {
            _hub = hub;
            _logger = logger;
        }

        public Task NotifyTaskCreated(Guid spaceId, TaskDTO task)
            => HubSafe.RunAsync(_logger, nameof(NotifyTaskCreated),
                () => _hub.Clients.Group(HubGroups.Space(spaceId)).TaskCreated(task));

        public Task NotifyTaskUpdated(Guid spaceId, TaskDTO task)
            => HubSafe.RunAsync(_logger, nameof(NotifyTaskUpdated),
                () => _hub.Clients.Group(HubGroups.Space(spaceId)).TaskUpdated(task));

        public Task NotifyTaskDeleted(Guid spaceId, Guid taskId)
            => HubSafe.RunAsync(_logger, nameof(NotifyTaskDeleted),
                () => _hub.Clients.Group(HubGroups.Space(spaceId)).TaskDeleted(taskId));
    }
}