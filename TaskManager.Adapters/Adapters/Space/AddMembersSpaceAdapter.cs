using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Diagnostics;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.Space
{
    public class AddMembersSpaceAdapter : IAddMembersSpacePort
    {
        private readonly DbContextTaskManager _context;

        public AddMembersSpaceAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, ICollection<string> members)
        {
            var response = new SimpleResponseModel();
            try
            {
                if (!await _context.Space.AnyAsync(x => x.Id == spaceId))
                {
                    response.Status = ResponseStatusEnum.NotFound;
                    response.Message = "Espaço não encontrado.";
                    return response;
                }

                if (members == null || !members.Any())
                {
                    response.Status = ResponseStatusEnum.Error;
                    response.Message = "Nenhum membro fornecido para adição.";
                    return response;
                }

                var normalizedEmails = members
                    .Select(x => x.Trim().ToLower())
                    .Distinct()
                    .ToList();

                var users = await _context.User
                    .Where(u => normalizedEmails.Contains(u.Email.Value)
                                && u.Status == UserStatusEnum.Active)
                    .Select(u => new { u.Id, Email = u.Email.Value })
                    .ToListAsync();

                var foundEmails = users.Select(x => x.Email).ToHashSet();

                var notFoundEmails = normalizedEmails
                    .Where(e => !foundEmails.Contains(e))
                    .ToList();

                var existingMemberIds = await _context.SpaceMember
                    .Where(sm => sm.SpaceId == spaceId)
                    .Select(sm => sm.UserId)
                    .ToListAsync();

                var newMembers = users
                    .Where(u => !existingMemberIds.Contains(u.Id))
                    .Select(u => new SpaceMemberEntity(spaceId, u.Id))
                    .ToList();

                if (newMembers.Any())
                {
                    await _context.SpaceMember.AddRangeAsync(newMembers);
                    await _context.SaveChangesAsync();
                }

                response.Status = ResponseStatusEnum.Success;
                response.Message = notFoundEmails.Any()
                    ? $"Membros adicionados. Os seguintes e-mails não foram encontrados: {string.Join(", ", notFoundEmails)}."
                    : "Todos os membros foram adicionados com sucesso.";

                return response;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                throw new Exception("Ocorreu um erro ao adicionar os membros ao espaço.");
            }
        }
    }
}