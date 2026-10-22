using TaskManager.Core.DTOs;
using TaskManager.Core.Ports.Notifications;

namespace TaskManager.API.Hubs.Notifiers
{
    /// <summary>
    /// Ponte temporária: implementa o antigo ISignalRNotifier delegando para os notifiers novos.
    /// Apague este arquivo (e o ISignalRNotifier) depois de migrar os use cases.
    /// </summary>
    public sealed class LegacySignalRNotifier : ISignalRNotifier
    {
        private readonly ITaskNotifier _tasks;
        private readonly ISpaceNotifier _spaces;

        public LegacySignalRNotifier(ITaskNotifier tasks, ISpaceNotifier spaces)
        {
            _tasks = tasks;
            _spaces = spaces;
        }

        public Task NotifyTaskCreated(Guid spaceId, TaskDTO task) => _tasks.NotifyTaskCreated(spaceId, task);
        public Task NotifyTaskUpdated(Guid spaceId, TaskDTO task) => _tasks.NotifyTaskUpdated(spaceId, task);
        public Task NotifyTaskDeleted(Guid spaceId, Guid taskId) => _tasks.NotifyTaskDeleted(spaceId, taskId);
        public Task NotifySpaceUpdated(Guid spaceId) => _spaces.NotifySpaceUpdated(spaceId);
    }
}