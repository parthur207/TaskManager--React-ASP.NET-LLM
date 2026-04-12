using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.DTOs;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    public class GetTasksBySpace : IGetTasksBySpaceUseCase
    {
        private readonly ICurrentUserPort _currentUserPort;
        public GetTasksBySpace(ICurrentUserPort currentUserPort)
        {
            _currentUserPort = currentUserPort;
        }

        public async Task<ResponseModel<IEnumerable<TaskDTO>>> ExecuteAsync()
        { 
            var Response = new ResponseModel<IEnumerable<TaskDTO>>();

            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Message = "Login expirado.";
                Response.Status = ResponseStatusEnum.Unauthorized;
                return Response;
            }

            throw new NotImplementedException();
        }
    }
}
