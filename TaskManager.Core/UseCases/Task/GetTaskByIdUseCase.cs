using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.DTOs;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.Ports.Task;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    public class GetTaskByIdUseCase : IGetTaskByIdUseCase
    {
        private readonly ICurrentUserPort _currentUserPort;
        private readonly IGetTaskByIdPort _getTaskByIdPort;
        public GetTaskByIdUseCase(IGetTaskByIdPort getTaskByIdPort, ICurrentUserPort currentUserPort)
        {
            _getTaskByIdPort = getTaskByIdPort;
            _currentUserPort = currentUserPort;
        }

        public async Task<ResponseModel<TaskDTO>> ExecuteAsync(Guid TaskId, Guid UserId)
        {
            var Response = new ResponseModel<TaskDTO>();

            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Message = "Login expirado.";
                Response.Status = ResponseStatusEnum.Unauthorized;
                return Response;
            }
            return _getTaskByIdPort.ExecuteAsync(TaskId, UserId);
        }
    }
}
