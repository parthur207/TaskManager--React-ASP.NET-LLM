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
using TaskManager.Core.Models.Task;
using TaskManager.Core.Ports.Caching;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.Task
{
    public class SearchTaskAdapter : ISearchTaskPort
    {
        private readonly DbContextTaskManager _context;
        private readonly ICachingPort _cachingPort;

        public SearchTaskAdapter(DbContextTaskManager context, ICachingPort cachingPort)
        {
            _context = context;
            _cachingPort = cachingPort;
        }

        public async Task<ResponseModel<List<TaskEntity>>> ExecuteAsync(SearchTaskModel model, Guid UserId)
        {
            var Response = new ResponseModel<List<TaskEntity>>();
            try
            {
                var responseCache= await _cachingPort.GetAsync<List<TaskEntity>>(GenerateSearchCacheKey(UserId, model));

                if (responseCache != null)
                {
                    Response.Content = responseCache;
                    Response.Status = ResponseStatusEnum.Success;
                    return Response;
                }

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

                await _cachingPort.SetAsync(GenerateSearchCacheKey(UserId, model), results, TimeSpan.FromMinutes(5));

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
                Debug.Assert(false, "Erro:" + ex.Message);
                throw new Exception("Ocorreu um erro inesperado.");
            }
        }

        private string GenerateSearchCacheKey(Guid userId, SearchTaskModel model)
        {
            var filterKey = $"cat_{model.Category}_status_{model.StatusEnum}_from_{model.From}_to_{model.To}";
            return $"searchTasks_{userId}_{filterKey.GetHashCode()}";
        }
    }
}
