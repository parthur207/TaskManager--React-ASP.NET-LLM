using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.DTOs;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Adapters.Space
{
    public class GetUserSpacesDetailsAdapter : IGetUserSpacesDetailsPort
    {
        private readonly DbContextTaskManager _context;
        public async Task<ResponseModel<SpaceDTO>> ExecuteAsync(Guid userId)
        {
            var Response = new ResponseModel<SpaceDTO>();
            try
            {

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw new Exception("Ocorreu um erro inesperado ao tentar obter os detalhes dos espaços do usuário.");
            }

        }
    }
}
