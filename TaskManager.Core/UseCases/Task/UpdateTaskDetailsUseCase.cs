using TaskManager.Core.Enums;
using TaskManager.Core.Mappers;
using TaskManager.Core.Models.Task;
using TaskManager.Core.Ports.Notifications;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.Ports.ReadServices;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    public class UpdateTaskDetailsUseCase : IUpdateTaskDetailsUseCase
    {
        private readonly IUpdateTaskDetailsPort _updateTaskDetailsPort;
        private readonly IGetTaskByIdPort _getTaskByIdPort;
        private readonly ICurrentUserPort _currentUserPort;
        private readonly IUserQueryPort _userQuery;
        private readonly ISpaceMembershipQueryPort _membership;
        private readonly ISignalRNotifier _notifier;

        public UpdateTaskDetailsUseCase(
            IUpdateTaskDetailsPort updateTaskDetailsPort,
            IGetTaskByIdPort getTaskByIdPort,
            ICurrentUserPort currentUserPort,
            IUserQueryPort userQuery,
            ISpaceMembershipQueryPort membership,
            ISignalRNotifier notifier)
        {
            _updateTaskDetailsPort = updateTaskDetailsPort;
            _getTaskByIdPort = getTaskByIdPort;
            _currentUserPort = currentUserPort;
            _userQuery = userQuery;
            _membership = membership;
            _notifier = notifier;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(UpdateTaskModel model)
        {
            var response = new SimpleResponseModel();

            if (!_currentUserPort.IsAuthenticated)
            {
                response.Message = "Sessão expirada. Realize o login novamente.";
                response.Status = ResponseStatusEnum.Unauthorized;
                return response;
            }

            if (model is null || model.Id == Guid.Empty)
            {
                response.Message = "Dados inválidos para atualização da tarefa.";
                response.Status = ResponseStatusEnum.Error;
                return response;
            }

            var taskResponse = await _getTaskByIdPort.ExecuteAsync(model.Id, _currentUserPort.UserId);
            if (taskResponse.Status != ResponseStatusEnum.Success || taskResponse.Content is null)
            {
                response.Status = taskResponse.Status;
                response.Message = taskResponse.Message;
                return response;
            }

            var newTitle = model.Title ?? taskResponse.Content.Title;
            var newDesc = model.Description ?? taskResponse.Content.Description;
            
            taskResponse.Content.UpdateTitleOrDescription(newTitle, newDesc);

            if (model.Status.HasValue 
                && model.Status.Value != taskResponse.Content.StatusEnum)
                    taskResponse.Content.UpdateStatusTask(model.Status.Value);

            if (model.Term.HasValue
                && model.Term.Value != taskResponse.Content.Term)
                    taskResponse.Content.UpdateTerm(model.Term.Value);
            

                if (!string.IsNullOrWhiteSpace(model.ResponsibleUserEmail))
                {
                    var userResponse = await _userQuery.GetUserByEmailAsync(model.ResponsibleUserEmail);
                    if (userResponse.Status != ResponseStatusEnum.Success || userResponse.Content is null)
                    {
                        response.Status = ResponseStatusEnum.NotFound;
                        response.Message = "Usuário responsável não encontrado.";
                        return response;
                    }

                    var memberCheck = await _membership.IsUserMemberAsync(userResponse.Content.Id, taskResponse.Content.SpaceId);
                    if (!memberCheck.Content)
                    {
                        response.Status = ResponseStatusEnum.Error;
                        response.Message = "O usuário responsável não é membro deste espaço.";
                        return response;
                    }

                    if (taskResponse.Content.ResponsibleUserId != userResponse.Content.Id)
                        taskResponse.Content.AssignResponsibleUser(userResponse.Content.Id);
                }

            var responseRepository = await _updateTaskDetailsPort
                .ExecuteAsync(_currentUserPort.UserId, taskResponse.Content);

            if(responseRepository.Status == ResponseStatusEnum.Success)
                await _notifier.NotifyTaskUpdated(model.SpaceId, TaskMapper.EntityToDTO(taskResponse.Content));

            return responseRepository;
        }
    }
}