using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.Ports.Persistence.TaskCategory
{
    public interface IDeleteTaskCategoryPort
    {
        Task<SimpleResponseModel> ExecuteAsync(Guid taskId, Guid userId);
    }
}
