using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
    public class AddMembersSpaceAdapter : IUpdateMembersSpacePort
    {
        private readonly DbContextTaskManager _context;
        public AddMembersSpaceAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, ICollection<string> Members)
        {
            var Response = new SimpleResponseModel();
            try
            {
                if (!await _context.Space
                    .AnyAsync(x=>x.Id == spaceId))
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Espaço não encontrado.";
                    return Response;
                }

                if (Members == null || !Members.Any())
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Nenhum membro fornecido para atualização.";
                    return Response;
                }





            }
            catch(Exception ex)
            {
                Debug.Assert(false, "Erro: " + ex.Message);
                throw new Exception("Ocorreu um erro ao atualizar os membros do espaço.");
            }   
        }
    }
}
