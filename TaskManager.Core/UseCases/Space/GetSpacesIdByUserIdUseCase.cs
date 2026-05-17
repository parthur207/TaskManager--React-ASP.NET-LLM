using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.Space.Interfaces;

namespace TaskManager.Core.UseCases.Space
{
    public class GetSpacesIdByUserIdUseCase : IGetSpacesIdByUserIdUseCase
    {
        private readonly ICurrentUserPort _currentUserPort;
        
        public GetSpacesIdByUserIdUseCase(ICurrentUserPort currentUserPort)
        {
            _currentUserPort = currentUserPort;
        }
        public async Task<ResponseModel<IEnumerable<Guid>>> ExecuteAsync()
        {
            var Response= new ResponseModel<IEnumerable<Guid>>();

            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Status = ResponseStatusEnum.Unauthorized;
                Response.Message = "Erro. Efetue o login novamente.";
                return Response;
            }


            throw new NotImplementedException();
        }
    }
}
