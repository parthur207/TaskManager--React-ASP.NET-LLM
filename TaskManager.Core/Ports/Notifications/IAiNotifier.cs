using TaskManager.Core.DTOs;
using TaskManager.Core.Enums;

namespace TaskManager.Core.Ports.Notifications
{
   
    public interface IAiNotifier
    {
        Task NotifyJobStarted(Guid spaceId, AiJobKind kind, Guid referenceId);
        Task NotifyJobFailed(Guid spaceId, AiJobKind kind, Guid referenceId, string message);
        Task NotifyPriorityReady(Guid spaceId, IEnumerable<AI_PriorityTasksDTO> priorities);
        Task NotifyExecutionPlanReady(Guid spaceId, Guid taskId, string executionPlan);
        Task NotifyChildTasksSuggested(Guid userId, Guid taskId, IEnumerable<TaskChildrenDTO> children);
    }
}