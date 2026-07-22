using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.TaskCategory;
using TaskManager.Core.Ports.Caching;
using TaskManager.Core.Ports.Persistence.TaskCategory;
using TaskManager.Core.Ports.ReadServices;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.TaskCategory
{
    public class UpdateTaskCategoryAdapter : IUpdateTaskCategoryPort
    {
        private readonly DbContextTaskManager _dbContext;
        private readonly ICurrentUserPort _currentUserPort;
        private readonly ICachingPort _cachingPort;
        public UpdateTaskCategoryAdapter(DbContextTaskManager dbContext, ICurrentUserPort currentUserPort, ICachingPort cachingPort)
        {
            _dbContext = dbContext;
            _currentUserPort = currentUserPort;
            _cachingPort = cachingPort;
        }
        public async Task<SimpleResponseModel> ExecuteAsync(UpdateTaskCategoryModel model)
        {
            var Response = new SimpleResponseModel();
            try
            {
                var userId= _currentUserPort.UserId;

                var taskCategoryEntity = await _dbContext
                    .TaskCategory
                    .FirstOrDefaultAsync(x=>x.Id == model.TaskCategoryId 
                    && x.OwnerId == userId);
                
                if (taskCategoryEntity is null)
                {
                    Response.Message= "A categoria não foi encontrada, ou você não possui autorização para atualiza-la..";
                    Response.Status= ResponseStatusEnum.Error;
                    return Response;
                }

                taskCategoryEntity.UpdateCategoryName(model.newName);
                _dbContext.Update(taskCategoryEntity);
                await _dbContext.SaveChangesAsync();

                await _cachingPort.RemoveAsync($"taskCategory_{taskCategoryEntity.Id}");
                await _cachingPort.RemoveAsync($"taskCategories_{taskCategoryEntity.SpaceId}");
                await _cachingPort.RemoveAsync($"Space_{taskCategoryEntity.SpaceId}");
                await _cachingPort.RemoveAsync($"spaceMember_{userId}");

                Response.Message = "O nome da categoria foi atualizado com sucesso.";
                Response.Status = ResponseStatusEnum.Success;
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
