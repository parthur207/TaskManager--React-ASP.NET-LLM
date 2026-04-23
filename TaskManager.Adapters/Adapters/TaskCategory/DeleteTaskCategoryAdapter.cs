using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.TaskCategory;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Adapters.TaskCategory
{
    public class DeleteTaskCategoryAdapter : IDeleteTaskCategoryPort
    {
        private readonly DbContextTaskManager _context;
        public DeleteTaskCategoryAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid taskCategoryId, Guid userId)
        {
            try
            {
                var Response= new SimpleResponseModel();

                if (taskCategoryId.Equals(Guid.Empty))
                {
                    Response.Message = "Operação inválida.";
                    Response.Status = ResponseStatusEnum.Error;
                    return Response;
                }

                var taskCategory = await _context.TaskCategory.FirstOrDefaultAsync(x => x.Id == taskCategoryId
                && x.UserId == userId);

                if (taskCategory is null)
                {
                    Response.Status = ResponseStatusEnum.Unauthorized;
                    Response.Message = "Operação inválida.";
                    return Response;
                }

                 _context.TaskCategory.Remove(taskCategory);
                await _context.SaveChangesAsync();

                return Response;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro inesperado.");
                Debug.Assert(false, ex.Message);
            }
        }
    }
}
