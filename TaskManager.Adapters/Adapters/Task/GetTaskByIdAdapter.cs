using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Caching;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Adapters.Adapters.Task
{
    public class GetTaskByIdAdapter : IGetTaskByIdPort
    {
        private readonly DbContextTaskManager _context;
        private readonly ICachingPort _cachingPort;

        public GetTaskByIdAdapter(DbContextTaskManager context, ICachingPort cachingPort = null)
        {
            _context = context;
            _cachingPort = cachingPort;
        }

        public async Task<ResponseModel<TaskEntity>> ExecuteAsync(Guid TaskId, Guid UserId)
        {
            var Response = new ResponseModel<TaskEntity>();
            try
            {
                if (TaskId == Guid.Empty)
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "ID da tarefa inválido.";
                    return Response;
                }

                var responseCache = await _cachingPort
                    .GetAsync<TaskEntity?>($"task_{TaskId}");
                
                if (responseCache != null)
                {
                    Response.Content = responseCache;
                    Response.Status = ResponseStatusEnum.Success;
                    return Response;
                }

                var task = await _context.Task
                    .Include(t => t.OwnerUser)
                    .Include(t => t.ResponsibleUser)
                    .Include(t => t.Category)
                    .Include(t => t.Space)
                    .Include(x=>x.TaskChildrens)
                    .FirstOrDefaultAsync(t => t.Id == TaskId);

                if (task is null)
                {
                    Response.Status = ResponseStatusEnum.NotFound;
                    Response.Message = "Tarefa não encontrada.";
                    return Response;
                }

                if (task.OwnerId != UserId && task.ResponsibleUserId != UserId)
                {
                    Response.Status = ResponseStatusEnum.Unauthorized;
                    Response.Message = "Usuário sem permissão para visualizar esta tarefa.";
                    return Response;
                }

                await _cachingPort.SetAsync($"task_{TaskId}", task, TimeSpan.FromMinutes(5));

                Response.Status = ResponseStatusEnum.Success;
                Response.Content = task;
                return Response;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "Erro:" + ex.Message);
                throw new Exception("Ocorreu um erro inesperado.");
            }
        }
    }
}
