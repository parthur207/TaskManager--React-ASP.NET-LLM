using System.Security.Cryptography;
using TaskManager.Core.Enums;
using TaskManager.Core.Models;
using TaskManager.Core.Ports.ReadServices;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.Ports.Task;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    public class CreateTaskUseCase : ICreateTaskUseCase
    {
        private readonly ICreateTaskPort _createTaskPort;
        private readonly ICurrentUserPort _currentUserPort;
        private readonly IUserQueryPort _userQuery;
        private readonly ISpaceMembershipPort _membership;
        public CreateTaskUseCase(ICreateTaskPort createTaskPort, IUserQueryPort userQuery, ISpaceMembershipPort membership, ICurrentUserPort currentUserPort)
        {
            _createTaskPort = createTaskPort;
            _userQuery = userQuery;
            _membership = membership;
            _currentUserPort = currentUserPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(CreateTaskModel model)
        {
            var Response = new SimpleResponseModel();

            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Message = "Login expirado.";
                Response.Status = ResponseStatusEnum.Unauthorized;
                return Response;
            }

            if (model is null)
            {
                Response.Message = "Erro. Dados nulos.";
                Response.Status = ResponseStatusEnum.Error;
                return Response;
            }
            
            if (!string.IsNullOrWhiteSpace(model.ResponsibleEmail)
                && _userQuery.UserExists(model.ResponsibleEmail).Result.Content)
            {
                Response.Message="Erro. O usuário responsável não existe.";
                Response.Status= ResponseStatusEnum.Error;
                return Response;
            }

            var responseRepository = await _createTaskPort.ExecuteAsync(model);

            if (responseRepository.S)
            {

            }

            return Response;
        }
    }
}
