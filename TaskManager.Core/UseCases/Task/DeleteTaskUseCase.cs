using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Notifications;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    public class DeleteTaskUseCase : IDeleteTaskUseCase
    {
        private readonly IDeleteTaskPort _deleteTaskPort;
        private readonly ICurrentUserPort _currentUserPort;
        private readonly ISpaceNotifier _notifier;

        public DeleteTaskUseCase(IDeleteTaskPort deleteTaskPort, ICurrentUserPort currentUserPort, ISpaceNotifier notifier)
        {
            _deleteTaskPort = deleteTaskPort;
            _currentUserPort = currentUserPort;
            _notifier = notifier;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, Guid taskId)
        {
            var response = new SimpleResponseModel();

            if (!_currentUserPort.IsAuthenticated)
            {
                response.Message = "Login expirado.";
                response.Status = ResponseStatusEnum.Unauthorized;
                return response;
            }

            if (taskId == Guid.Empty)
            {
                response.Message = "ID da tarefa inválido.";
                response.Status = ResponseStatusEnum.Error;
                return response;
            }

            var repositoryResponse = await _deleteTaskPort.ExecuteAsync(taskId, _currentUserPort.UserId);

            await _notifier.NotifyTaskDeleted(spaceId,taskId);
            return repositoryResponse;
        }
    }
}