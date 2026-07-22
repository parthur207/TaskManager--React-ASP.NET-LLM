using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Caching;
using TaskManager.Core.Ports.Persistence.TaskCategory;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.TaskCategory
{
    public class DeleteTaskCategoryAdapter : IDeleteTaskCategoryPort
    {
        private readonly DbContextTaskManager _context;
        private readonly ICachingPort _cachingPort;
        public DeleteTaskCategoryAdapter(DbContextTaskManager context, ICachingPort cachingPort)
        {
            _context = context;
            _cachingPort = cachingPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid taskCategoryId, Guid userId)
        {
            var response = new SimpleResponseModel();
            try
            {
                if (taskCategoryId == Guid.Empty)
                {
                    response.Message = "Operação inválida.";
                    response.Status = ResponseStatusEnum.Error;
                    return response;
                }

                var taskCategory = await _context.TaskCategory
                    .FirstOrDefaultAsync(x => x.Id == taskCategoryId && x.OwnerId == userId);

                if (taskCategory is null)
                {
                    response.Status = ResponseStatusEnum.Unauthorized;
                    response.Message = "Categoria não encontrada ou sem permissão para excluí-la.";
                    return response;
                }

                _context.TaskCategory.Remove(taskCategory);
                await _context.SaveChangesAsync();

                await _cachingPort.RemoveAsync($"taskCategory_{taskCategoryId}");
                await _cachingPort.RemoveAsync($"taskCategories_{taskCategory.SpaceId}");
                await _cachingPort.RemoveAsync($"Space_{taskCategory.SpaceId}");
                await _cachingPort.RemoveAsync($"spaceMember_{userId}");

                response.Status = ResponseStatusEnum.Success;
                response.Message = "Categoria excluída com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                throw new Exception("Ocorreu um erro inesperado ao excluir a categoria.");
            }
        }
    }
}