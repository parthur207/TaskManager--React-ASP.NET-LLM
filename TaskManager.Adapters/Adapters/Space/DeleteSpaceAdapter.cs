using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Adapters.Space
{
    public class DeleteSpaceAdapter : IDeleteSpacePort
    {

        private readonly DbContextTaskManager _context;

        public DeleteSpaceAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, Guid userId)
        {
            var Response = new SimpleResponseModel();
            try
            {
                if (!await _context.Space.AnyAsync(x => x.Id == spaceId && x.OwnerId == userId))
                {
                    Response.Status = ResponseStatusEnum.Unauthorized;
                    Response.Message = "Erro ao efetuar a ação.";
                    return Response;
                }

                await _context.Space.Where(x => x.Id == spaceId).ExecuteDeleteAsync();

                Response.Status = ResponseStatusEnum.Success;
                Response.Message = "Espaço excluído com sucesso.";
                return Response;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                throw new Exception("Ocorreu um erro inesperado ao tentar excluir o espaço.");
            }
        }
    }
}
