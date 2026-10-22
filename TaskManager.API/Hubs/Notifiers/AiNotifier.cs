using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;
using TaskManager.API.Hubs.RealTime;
using TaskManager.API.Realtime;
using TaskManager.Core.DTOs;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Notifications;

namespace TaskManager.API.Hubs.Notifiers
{
    public sealed class AiNotifier : IAiNotifier
    {
        private readonly IHubContext<TaskManagerHub, ITaskManagerHubClient> _hub;
        private readonly ILogger<AiNotifier> _logger;

        public AiNotifier(IHubContext<TaskManagerHub, ITaskManagerHubClient> hub, ILogger<AiNotifier> logger)
        {
            _hub = hub;
            _logger = logger;
        }

        public Task NotifyJobStarted(Guid spaceId, AiJobKind kind, Guid referenceId)
            => HubSafe.RunAsync(_logger, nameof(NotifyJobStarted),
                () => _hub.Clients.Group(HubGroups.Space(spaceId)).AiJobStarted(kind, referenceId));

        public Task NotifyJobFailed(Guid spaceId, AiJobKind kind, Guid referenceId, string message)
            => HubSafe.RunAsync(_logger, nameof(NotifyJobFailed),
                () => _hub.Clients.Group(HubGroups.Space(spaceId)).AiJobFailed(kind, referenceId, message));

        public Task NotifyPriorityReady(Guid spaceId, IEnumerable<AI_PriorityTasksDTO> priorities)
            => HubSafe.RunAsync(_logger, nameof(NotifyPriorityReady),
                () => _hub.Clients.Group(HubGroups.Space(spaceId)).AiPriorityReady(spaceId, priorities));

        public Task NotifyExecutionPlanReady(Guid spaceId, Guid taskId, string executionPlan)
            => HubSafe.RunAsync(_logger, nameof(NotifyExecutionPlanReady),
                () => _hub.Clients.Group(HubGroups.Space(spaceId)).AiExecutionPlanReady(spaceId, taskId, executionPlan));

        // sugestão pessoal: só quem pediu (todas as abas/dispositivos dele)
        public Task NotifyChildTasksSuggested(Guid userId, Guid taskId, IEnumerable<TaskChildrenDTO> children)
            => HubSafe.RunAsync(_logger, nameof(NotifyChildTasksSuggested),
                () => _hub.Clients.User(userId.ToString()).AiChildTasksSuggested(taskId, children));
    }
}