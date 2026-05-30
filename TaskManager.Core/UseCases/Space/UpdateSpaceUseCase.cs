using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.Space;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.Space.Interfaces;

namespace TaskManager.Core.UseCases.Space
{
    public class UpdateSpaceUseCase : IUpdateSpaceUseCase
    {
        private readonly IUpdateSpacePort _updateSpacePort;
        private readonly ICurrentUserPort _currentUserPort;
        public UpdateSpaceUseCase(IUpdateSpacePort updateSpacePort, ICurrentUserPort currentUserPort)
        {
            _updateSpacePort = updateSpacePort;
            _currentUserPort = currentUserPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, UpdateSpaceModel model)
        {
            var Response = new SimpleResponseModel();

            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Status = ResponseStatusEnum.Unauthorized;
                Response.Message = "Erro. Efetue o login novamente.";
                return Response;
            }

            if (model.NewName is null)
            {
                Response.Status = ResponseStatusEnum.Error;
                Response.Message = "Erro. O nome do espaço é obrigatório.";
                return Response;
            }

            var responseRepository = await _updateSpacePort
                .ExecuteAsync(spaceId, model.NewName);
           
            return responseRepository;
        }
    }
}
