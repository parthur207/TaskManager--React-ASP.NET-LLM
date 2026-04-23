using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.TaskCategory;
using TaskManager.Core.Ports.Persistence.TaskCategory;
using TaskManager.Core.Ports.ReadServices;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Adapters.TaskCategory
{
    public class UpdateTaskCategoryAdapter : IUpdateTaskCategoryPort
    {
        private readonly DbContextTaskManager _dbContext;
        private readonly ICurrentUserPort _currentUserPort;
        public UpdateTaskCategoryAdapter(DbContextTaskManager dbContext, ICurrentUserPort currentUserPort)
        {
            _dbContext = dbContext;
            _currentUserPort = currentUserPort;
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
                    && x.UserId == userId);
                
                if (taskCategoryEntity is null)
                {
                    Response.Message= "A categoria não foi encontrada, ou você não possui autorização para atualiza-la..";
                    Response.Status= ResponseStatusEnum.Error;
                    return Response;
                }

                taskCategoryEntity.UpdateCategoryName(model.newName);
                _dbContext.Update(taskCategoryEntity);
                await _dbContext.SaveChangesAsync();

                Response.Message = "O nome da categoria foi atualizado com sucesso.";
                Response.Status = ResponseStatusEnum.Success;
                return Response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
