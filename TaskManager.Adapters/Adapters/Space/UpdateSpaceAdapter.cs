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
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.Space
{
    public class UpdateSpaceAdapter : IUpdateSpacePort
    {
        private readonly DbContextTaskManager _context;

        public UpdateSpaceAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, string newName)
        {
            var Response = new SimpleResponseModel();
            try
            {
                if (!await _context.Space.AnyAsync(s => s.Id == spaceId))
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Erro. O espaço não foi encontrado.";
                    return Response;
                }
                await _context.Space.Where(s => s.Id == spaceId)
                    .ExecuteUpdateAsync(s => s.SetProperty(s => s.Name, newName));
                await _context.SaveChangesAsync();

                Response.Status = ResponseStatusEnum.Success;
                Response.Message = "Espaço atualizado com sucesso.";
                return Response;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "Erro: " + ex.Message);
                throw new Exception("Ocorreu um erro ao atualizar o espaço.");
            }
        }
    }
}
