using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.UseCases.TaskCategory.Interfaces
{
    public interface IGetAllTaskCategoryUseCase
    {
        Task<ResponseModel<IEnumerable<TaskCategoryDTO>>> ExecuteAsync(Guid spaceId);
    }
}
