using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.Space.Interfaces;

namespace TaskManager.Core.UseCases.Space
{
    public class DeleteSpaceUseCase : IDeleteSpaceUseCase
    {
        private readonly IDeleteSpacePort _deleteSpacePort;
        private readonly ICurrentUserPort _currentUserPort;
        public DeleteSpaceUseCase(IDeleteSpacePort deleteSpacePort, ICurrentUserPort currentUserPort)
        {
            _deleteSpacePort = deleteSpacePort;
            _currentUserPort = currentUserPort;
        }
        public async Task<SimpleResponseModel> ExecuteAsync(Guid spaceId)
        {
            var Response = new SimpleResponseModel();

            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Status = ResponseStatusEnum.Unauthorized;
                Response.Message = "Você não está autenticado. Efetue o login novamente.";
                return Response;
            }

            Response = await _deleteSpacePort.ExecuteAsync(spaceId, _currentUserPort.UserId);

            return Response;
        }
    }
}
