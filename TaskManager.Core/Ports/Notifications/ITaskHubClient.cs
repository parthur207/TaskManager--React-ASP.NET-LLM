using TaskManager.Core.DTOs;

namespace TaskManager.Core.Ports.Notifications
{
    public interface ITaskHubClient
    {
        Task TaskCreated(TaskDTO task);
        Task TaskUpdated(TaskDTO task);
        Task TaskDeleted(Guid taskId);
        Task SpaceUpdated(Guid spaceId);
    }
}