using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.User;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Adapters.User
{
    public class GetDataUserProfileAdapter : IGetDataUserProfilePort
    {
        private readonly DbContextTaskManager _context;
        public GetDataUserProfileAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<ResponseModel<UserEntity>> GetDataUserProfileAsync(Guid userId)
        {
            var Response= new ResponseModel<UserEntity>();
            try
            {
                if (!await _context.User.AnyAsync(x=>x.Id==userId))
                {
                    Response.Message= "Usuário não encontrado";
                    Response.Status = ResponseStatusEnum.Error;
                    return Response;
                }

                var user = await _context
                    .User
                    .Where(x => x.Id == userId)
                    .Include(x => x.Spaces)
                        .ThenInclude(x=>x.Space)
                    .FirstOrDefaultAsync();

                Response.Content = user;
                Response.Status = ResponseStatusEnum.Success;
                return Response;
            }
            catch (Exception ex)
            {
                Debug.Assert(false,"Erro: " +ex.Message);
                throw new Exception("Ocorreu um erro inesperado");
            }
        }
    }
}
