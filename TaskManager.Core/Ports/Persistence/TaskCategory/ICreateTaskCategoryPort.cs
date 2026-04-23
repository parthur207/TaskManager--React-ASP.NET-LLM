using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Models.TaskCategory;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.Ports.Persistence.TaskCategory
{
    public interface ICreateTaskCategoryPort
    {
        Task<SimpleResponseModel> ExecuteAsync(CreateTaskCategoryModel model, Guid userId);
    }
}
