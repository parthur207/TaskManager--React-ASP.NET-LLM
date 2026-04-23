using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Adapters.Task
{
    public class UpdateTaskDetailsAdapter : IUpdateTaskDetailsPort
    {
        private readonly DbContextTaskManager _context;
        public UpdateTaskDetailsAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(TaskEntity entity)
        {
            var Response = new SimpleResponseModel();
            try
            {
                if (entity is null)
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Entidade inválida ou nula.";
                    return Response;
                }

                var existing = await _context.Task.FirstOrDefaultAsync(t => t.Id == entity.Id);
                if (existing is null)
                {
                    Response.Status = ResponseStatusEnum.NotFound;
                    Response.Message = "Tarefa não encontrada.";
                    return Response;
                }

                existing.UpdateTitleOrDescription(entity.Title, entity.Description);
                if (!existing.StatusEnum.Equals(entity.StatusEnum))
                {
                    existing.UpdateStatusTask(entity.StatusEnum);
                }

                if (entity.ResponsibleUserId != null && entity.ResponsibleUserId != existing.ResponsibleUserId)
                {
                    existing.AssignResponsibleUser(entity.ResponsibleUserId.Value);
                }

                _context.Task.Update(existing);
                await _context.SaveChangesAsync();

                Response.Status = ResponseStatusEnum.Success;
                Response.Message = "Tarefa atualizada com sucesso.";
                return Response;
            }
            catch (ArgumentException ex)
            {
                Response.Status = ResponseStatusEnum.Error;
                Response.Message = ex.Message;
                return Response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro inesperado: {ex.Message}");
            }
        }
    }
}
