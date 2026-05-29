using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.Ports.Persistence.TaskCategory
{
    public interface IGetAllTaskCategoryPort
    {
        Task<ResponseModel<IEnumerable<TaskCategoryEntity>>> ExecuteAsync(Guid spaceId, Guid userId);
    }
}
