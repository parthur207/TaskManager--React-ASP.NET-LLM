using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.Task;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Adapters.Task
{
    public class SearchTaskAdapter : ISearchTaskPort
    {
        private readonly DbContextTaskManager _context;
        public SearchTaskAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<ResponseModel<List<TaskEntity>>> ExecuteAsync(SearchTaskModel model, Guid UserId)
        {
            var Response = new ResponseModel<List<TaskEntity>>();
            try
            {
                var query = _context.Task
                    .Include(t => t.OwnerUser)
                    .Include(t => t.ResponsibleUser)
                    .Include(t => t.Category)
                    .Include(t => t.Space)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(model?.Category))
                {
                    query = query.Where(t => t.Category != null && t.Category.Name == model.Category);
                }

                if (model?.StatusEnum != null)
                {
                    query = query.Where(t => t.StatusEnum == model.StatusEnum);
                }

                if (model?.From != null)
                {
                    query = query.Where(t => t.CreatedAt >= model.From.Value);
                }

                if (model?.To != null)
                {
                    query = query.Where(t => t.CreatedAt <= model.To.Value);
                }

                var userSpaces = await _context.SpaceMember
                    .Where(sm => sm.UserId == UserId)
                    .Select(sm => sm.SpaceId)
                    .ToListAsync();

                query = query.Where(t => userSpaces.Contains(t.SpaceId));

                var results = await query.ToListAsync();

                if (results == null || !results.Any())
                {
                    Response.Status = ResponseStatusEnum.NotFound;
                    Response.Message = "Nenhuma tarefa encontrada com os filtros informados.";
                    return Response;
                }

                Response.Status = ResponseStatusEnum.Success;
                Response.Content = results;
                return Response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro inesperado: {ex.Message}");
            }
        }
    }
}
