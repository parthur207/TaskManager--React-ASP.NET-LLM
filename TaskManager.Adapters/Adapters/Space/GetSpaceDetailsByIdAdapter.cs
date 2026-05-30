using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.Space
{
    public class GetSpaceDetailsByIdAdapter : IGetSpaceDetailsByIdPort
    {
        private readonly DbContextTaskManager _context;
        public GetSpaceDetailsByIdAdapter(DbContextTaskManager context)
        {
            _context = context;
        }
        public async Task<ResponseModel<SpaceEntity>> ExecuteAsync(Guid userId, Guid spaceId)
        {
            var Response = new ResponseModel<SpaceEntity>();

            try
            {
                if (!await _context.Space.AnyAsync(x=>x.Id == spaceId))
                {
                    Response.Message = "Erro. Espaço não encontrado.";
                    Response.Status = ResponseStatusEnum.Error;
                    return Response; 
                }

                if (!await _context.Space.AnyAsync(x=>x.Id == spaceId && x.Members.Any(m=>m.UserId == userId)))
                {
                    Response.Message = "Erro. Usuário não autorizado a acessar este espaço.";
                    Response.Status = ResponseStatusEnum.Unauthorized;
                    return Response;
                }

                var space = await _context.Space
                    .Include(x => x.Tasks)
                    .Include(x=>x.TaskCategories)
                    .Include(x=>x.Members)
                        .ThenInclude(x=>x.User)
                    .FirstOrDefaultAsync(x => x.Id == spaceId);

                Response.Content = space;
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
