using Azure;
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
    public class LeaveSpaceAdapter : ILeaveSpacePort
    {
        private readonly DbContextTaskManager _context;
        public LeaveSpaceAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, Guid userId)
        {
            var Response = new SimpleResponseModel();
            try
            {
                if (!await _context.Space.AnyAsync(x => x.Id == spaceId))
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Espaço não encontrado.";
                    return Response;
                }

                if (await _context.Space.AnyAsync(x=>x.OwnerId==userId))
                {
                    Response.Message= "O proprietário do espaço não pode sair do espaço. Considere excluir o espaço ou transferir a propriedade para outro usuário.";
                    Response.Status = ResponseStatusEnum.Error;
                    return Response;
                }

                var userleave = await _context.SpaceMember.FirstOrDefaultAsync(x => x.UserId == userId);

                _context.SpaceMember.Remove(userleave);
                await _context.SaveChangesAsync();

                Response.Message= "Você saiu do espaço!";
                Response.Status = ResponseStatusEnum.Success;
                return Response;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "Erro: "+ex.Message);
                throw new Exception("Ocorreu um erro inesperado.");
            }
        }
    }
}
