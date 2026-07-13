using TaskManager.Core.DTOs;

namespace TaskManager.Core.Ports.Notifications
{
    public interface ITaskManagerHubClient
    {
        Task TaskCreated(TaskDTO task);
        Task TaskUpdated(TaskDTO task);
        Task TaskDeleted(Guid taskId);
        Task SpaceUpdated(Guid spaceId);
    }
}