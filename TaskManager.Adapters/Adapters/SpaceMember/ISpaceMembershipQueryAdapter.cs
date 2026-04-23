using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.DTOs;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.ReadServices;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Adapters.SpaceMember
{
    public class SpaceMembershipQueryAdapter : ISpaceMembershipQueryPort
    {
        private readonly DbContextTaskManager _context;       
        public SpaceMembershipQueryAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<ResponseModel<IEnumerable<string>>> GetUsersEmailsInSpaceAsync(Guid spaceId, Guid userId)
        {
            var Response = new ResponseModel<IEnumerable<string>>();
            try
            {
                var isMemberResponse = await IsUserMemberAsync(userId, spaceId);
                if (!isMemberResponse.Content)
                {
                    Response.Message = "O usuário não é membro do espaço especificado.";
                    Response.Status = ResponseStatusEnum.Unauthorized;
                    return Response;
                }

                var usersEmails = await _context.SpaceMember
                    .Where(x => x.SpaceId == spaceId)
                    .Select(x => x.User.Email.Value)
                    .ToListAsync();

                if (usersEmails == null || !usersEmails.Any())
                {
                    Response.Message = "Nenhum usuário encontrado no espaço especificado.";
                    Response.Status = ResponseStatusEnum.NotFound;
                    return Response;
                }

                Response.Content = usersEmails;
                Response.Status = ResponseStatusEnum.Success;
                return Response;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro inesperado.");
                Debug.Assert(false, ex.Message);
            }
        }

        public async Task<ResponseModel<IEnumerable<Guid>>> GetUserSpacesAsync(Guid userId)
        {
            var Response = new ResponseModel<IEnumerable<Guid>>();
            try
            {
                if (userId == Guid.Empty)
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "O ID do usuário não pode ser vazio.";
                    return Response;
                }

                var userSpaces = await _context.SpaceMember
                    .Where(x => x.UserId == userId)
                    .Select(x => x.SpaceId)
                    .ToListAsync() ?? new List<Guid>();

                if (userSpaces == null || !userSpaces.Any())
                {
                    Response.Status = ResponseStatusEnum.NotFound;
                    Response.Message = "Nenhum espaço encontrado para o usuário especificado.";
                    return Response;
                }

                Response.Content = userSpaces;
                Response.Status = ResponseStatusEnum.Success;
                return Response;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro inesperado.");
                Debug.Assert(false, ex.Message);
            }
        }


        public async Task<ResponseModel<IEnumerable<SpaceDTO>>> GetUserSpacesDetailsAsync(Guid userId)
        {
            var Response = new ResponseModel<IEnumerable<SpaceDTO>>();
            try
            {
                if (userId == Guid.Empty)
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "O ID do usuário não pode ser vazio.";
                    return Response;
                }

                var userSpacesDetails = await _context.SpaceMember
                    .Where(x => x.UserId == userId)
                    .Select(x => new SpaceDTO
                    {
                        Id = x.Space.Id,
                        Name = x.Space.Name,
                        CreatedAt = x.Space.CreatedAt,
                        UpdatedAt = x.Space.UpdatedAt
                    })
                    .ToListAsync() ?? new List<SpaceDTO>();
                
                Response.Content = userSpacesDetails;
                Response.Status = ResponseStatusEnum.Success;
                return Response;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro inesperado.");
                Debug.Assert(false, ex.Message);
            }
        }

        public async Task<ResponseModel<bool>> IsUserMemberAsync(Guid userId, Guid spaceId)
        {
            var Response = new ResponseModel<bool>();
            try
            {
                if (!await _context.SpaceMember.AnyAsync(x => x.SpaceId == spaceId 
                && x.UserId == userId))
                {
                    Response.Status = ResponseStatusEnum.NotFound;
                    Response.Message = "O usuário não é membro do espaço especificado.";
                    Response.Content = false;
                    return Response;
                }

                Response.Status = ResponseStatusEnum.Success;
                Response.Content = true;
                return Response;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro inesperado.");
                Debug.Assert(false, ex.Message);
            }
        }
    }
}
