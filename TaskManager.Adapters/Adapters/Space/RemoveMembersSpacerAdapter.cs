using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.Space
{
    public class RemoveMembersSpacerAdapter : IRemoveMembersSpacePort
    {
        private readonly DbContextTaskManager _context;

        public RemoveMembersSpacerAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, ICollection<string> members)
        {
            var response = new SimpleResponseModel();
            try
            {
                if (members is null || !members.Any())
                {
                    response.Message = "Nenhum membro para remover.";
                    response.Status = ResponseStatusEnum.Error;
                    return response;
                }

                if (!await _context.Space.AnyAsync(s => s.Id == spaceId))
                {
                    response.Message = "Espaço não encontrado.";
                    response.Status = ResponseStatusEnum.NotFound;
                    return response;
                }

                var normalizedEmails = members
                    .Select(e => e.Trim().ToLower())
                    .Distinct()
                    .ToList();

                var memberIds = await _context.User
                    .Where(x => normalizedEmails.Contains(x.Email.Value)
                                && x.Spaces.Any(s => s.SpaceId == spaceId))
                    .Select(u => u.Id)
                    .ToHashSetAsync();

                if (!memberIds.Any())
                {
                    response.Message = "Nenhum dos e-mails informados corresponde a membros deste espaço.";
                    response.Status = ResponseStatusEnum.NotFound;
                    return response;
                }

                var toRemove = await _context.SpaceMember
                    .Where(sm => sm.SpaceId == spaceId && memberIds.Contains(sm.UserId))
                    .ToListAsync();

                _context.SpaceMember.RemoveRange(toRemove);
                await _context.SaveChangesAsync();

                response.Status = ResponseStatusEnum.Success;
                response.Message = "Membros removidos com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, $"Erro: {ex.Message}");
                throw new Exception("Ocorreu um erro ao remover os membros do espaço.");
            }
        }
    }
}