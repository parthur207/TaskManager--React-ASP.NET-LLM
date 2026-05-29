using TaskManager.Core.Enums;
using TaskManager.Core.Models.Task;
using TaskManager.Core.Ports.Persistence.Task;
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

        public UpdateTaskDetailsUseCase(
            IUpdateTaskDetailsPort updateTaskDetailsPort,
            IGetTaskByIdPort getTaskByIdPort,
            ICurrentUserPort currentUserPort)
        {
            _updateTaskDetailsPort = updateTaskDetailsPort;
            _getTaskByIdPort = getTaskByIdPort;
            _currentUserPort = currentUserPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(UpdateTaskModel model)
        {
            var Response = new SimpleResponseModel();

            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Message = "Login expirado.";
                Response.Status = ResponseStatusEnum.Unauthorized;
                return Response;
            }

            var properties = model.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.Name == nameof(model.Id))
                    continue;

                var value = property.GetValue(model);

                if (value is IDictionary<bool, string> stringDict)
                {
                    if (stringDict.TryGetValue(true, out var fieldValue))
                    {
                        Response = await _updateTaskDetailsPort
                            .UpdateTaskDetailAsync(model.Id, property.Name, fieldValue);
                    }
                }

                if (value is IDictionary<bool, DateTime> dateDict)
                {
                    if (dateDict.TryGetValue(true, out var fieldValue))
                    {

                    }
                }
            }
        }
    }
}