using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.Space.Interfaces;

namespace TaskManager.Core.UseCases.Space
{
    public class GetUserSpacesDetailsUsecase : IGetUserSpacesDetailsUsecase
    {
        private readonly ICurrentUserPort _currentUserPort;
        private readonly IGetUserSpacesDetailsPort _getUserSpacesDetailsPort;
        public GetUserSpacesDetailsUsecase(ICurrentUserPort currentUserPort, IGetUserSpacesDetailsPort getUserSpacesDetailsPort)
        {
            _currentUserPort = currentUserPort;
            _getUserSpacesDetailsPort = getUserSpacesDetailsPort;
        }
        public async Task<ResponseModel<SpaceDTO>> ExecuteAsync(Guid spaceId)
        {
            var Response = new ResponseModel<SpaceDTO>();

            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Status = ResponseStatusEnum.Unauthorized;
                Response.Message = "Erro. Efetue o login novamente.";
                return Response;
            }

            var ResponsePort = await _getUserSpacesDetailsPort.ExecuteAsync(spaceId);
            return ResponsePort;
        }
    }
}
