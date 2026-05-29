using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaskManager.Adapters.Persistence;
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
                var spaceExists = await _context.Space
                    .AnyAsync(x => x.Id == spaceId);

                if (!spaceExists)
                {
                    response.Status = ResponseStatusEnum.Error;
                    response.Message = "Espaço não encontrado.";
                    return response;
                }

                if (members == null || !members.Any())
                {
                    response.Status = ResponseStatusEnum.Error;
                    response.Message = "Nenhum membro fornecido para atualização.";
                    return response;
                }

                var normalizedEmails = members
                    .Select(x => x.Trim().ToLower())
                    .Distinct()
                    .ToList();

                var users = await _context.User
                    .Where(u => normalizedEmails.Contains(u.Email.Value))
                    .Select(u => new
                    {
                        u.Id,
                        Email = u.Email.Value
                    })
                    .ToListAsync();

                var foundEmails = users
                    .Select(x => x.Email)
                    .ToHashSet();

                var membersEmailInexistent = normalizedEmails
                    .Where(email => !foundEmails.Contains(email))
                    .ToList();

                var membersIds = users
                    .Select(x => x.Id)
                    .ToHashSet();

                // Aqui você adiciona os membros ao espaço

                // Aqui você pode disparar o envio de e-mail em background (ASYNC)

                response.Message = membersEmailInexistent.Any()
                    ? $"Os seguintes e-mails não foram encontrados: {string.Join(", ", membersEmailInexistent)}."
                    : "Todos os membros foram adicionados com sucesso.";

                response.Status = ResponseStatusEnum.Success;

                return response;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);

                throw new Exception("Ocorreu um erro ao atualizar os membros do espaço.");
            }
        }
    }
}