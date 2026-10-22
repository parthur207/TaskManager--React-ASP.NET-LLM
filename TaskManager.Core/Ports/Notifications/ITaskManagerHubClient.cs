using TaskManager.Core.DTOs;
using TaskManager.Core.Enums;

namespace TaskManager.API.Realtime
{
    public interface ITaskManagerHubClient
    {
        Task TaskCreated(TaskDTO task);
        Task TaskUpdated(TaskDTO task);
        Task TaskDeleted(Guid taskId);

        Task SpaceUpdated(Guid spaceId);
        Task SpaceDeleted(Guid spaceId);
        Task AddedToSpace(Guid spaceId);       
        Task RemovedFromSpace(Guid spaceId);   

        Task AiJobStarted(AiJobKind kind, Guid referenceId);
        Task AiJobFailed(AiJobKind kind, Guid referenceId, string message);
        Task AiPriorityReady(Guid spaceId, IEnumerable<AI_PriorityTasksDTO> priorities);
        Task AiExecutionPlanReady(Guid spaceId, Guid taskId, string executionPlan);
        Task AiChildTasksSuggested(Guid taskId, IEnumerable<TaskChildrenDTO> children); // alvo: usuário
    }
}