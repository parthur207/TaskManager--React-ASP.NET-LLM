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
using TaskManager.Core.Ports.Persistence.TaskCategory;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.TaskCategory.Interfaces;

namespace TaskManager.Adapters.Adapters.TaskCategory
{
    public class GetAllTaskCategoryPort : IGetAllTaskCategoryPort
    {
        private readonly DbContextTaskManager _context;
        public GetAllTaskCategoryPort(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<ResponseModel<IEnumerable<TaskCategoryEntity>>> ExecuteAsync(Guid spaceId, Guid userId)
        {
            var Response= new ResponseModel<IEnumerable<TaskCategoryEntity>>();
            try
            {
                if (!await _context.TaskCategory.AnyAsync(x => x.SpaceId == spaceId))
                {
                    Response.Message = "Nenhuma categoria de tarefa encontrada para o espaço especificado.";
                    Response.Status = ResponseStatusEnum.Error;
                    return Response;
                }

                if (!await _context.SpaceMember.AnyAsync(x => x.SpaceId ==spaceId && x.UserId == userId))
                {
                    Response.Message = "Você não possui permissão para este espaço.";
                    Response.Status = ResponseStatusEnum.Unauthorized;
                    return Response;
                }

                var taskCategories = await _context.TaskCategory
                    .Where(x => x.SpaceId == spaceId)
                    .ToListAsync();


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ocorreu um erro inesperado: "+ex);
                throw new Exception("Ocorreu um erro inesperado ao tentar coletar as categorias de tarefas deste espaço.");
            }

            return Response;
        }
    }
}
