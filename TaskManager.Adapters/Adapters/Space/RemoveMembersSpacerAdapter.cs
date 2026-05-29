using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Enums;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.Space
{
    public class RemoveMembersSpacerAdapter
    {
        private readonly DbContextTaskManager _context;

        public RemoveMembersSpacerAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, ICollection<string> Members)
        {
            var Response = new SimpleResponseModel();
            try
            {
                var MembersIds = new HashSet<Guid>();

                if (Members is null || !Members.Any())
                {
                    Response.Message= "Nenhum membro para remover.";
                    Response.Status = ResponseStatusEnum.Error;    
                    return Response;
                }

                if (!await _context.Space.AnyAsync(s => s.Id == spaceId))
                {
                    Response.Message = "Espaço não encontrado.";
                    Response.Status = ResponseStatusEnum.Error;
                    return Response;
                }

                MembersIds = await _context.User
                    .Where(x => Members.Contains(x.Email.Value)
                    && x.Spaces.Any(s => s.SpaceId == spaceId))
                    .Select(u => u.Id)
                    .ToHashSetAsync();

                _context.SpaceMember.RemoveRange(
                    await _context.SpaceMember
                    .Where(sm => sm.SpaceId == spaceId 
                    && MembersIds.Contains(sm.UserId))
                    .ToListAsync());

                await _context.SaveChangesAsync();

                Response.Status = ResponseStatusEnum.Success;
                Response.Message = "Membros removidos com sucesso.";
                return Response;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "Erro: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
