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

namespace TaskManager.Adapters.Adapters.User
{
    public class GetUsersBySpaceIdAdapter : IGetUsersBySpaceIdPort
    {
        private readonly DbContextTaskManager _dbContextTaskManager;

        public GetUsersBySpaceIdAdapter(DbContextTaskManager dbContextTaskManager)
        {
            _dbContextTaskManager = dbContextTaskManager;
        }

        public async Task<ResponseModel<IEnumerable<string>>> ExecuteAsync(Guid spaceId)
        {
            var Response= new ResponseModel<IEnumerable<string>>();
            try
            {
                if (!await _dbContextTaskManager.Space.AnyAsync(s => s.Id == spaceId))
                {
                    Response.Message = "Erro. Espaço não encontrado.";
                    Response.Status = ResponseStatusEnum.Error;
                    return Response;
                }

                var users =await _dbContextTaskManager.SpaceMember.Where(u => u.SpaceId == spaceId).Select(u => u.User.Email.Value).ToListAsync();

                if (users is null || !users.Any())
                {
                    Response.Message = "Nenhum usuário encontrado para o espaço especificado.";
                    Response.Status = ResponseStatusEnum.Error;
                    return Response;
                }

                Response.Content = users;
                Response.Status = ResponseStatusEnum.Success;
                return Response;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "Erro:"+ex.Message);
                throw new Exception("Ocorreu um erro inesperado.");
            }
        }
    }
}
