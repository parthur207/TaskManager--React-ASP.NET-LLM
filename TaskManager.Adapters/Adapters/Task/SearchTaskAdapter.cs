using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities;
using TaskManager.Core.Models.Task;
using TaskManager.Core.Ports.Task;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Persistence.Task
{
    public class SearchTaskAdapter : ISearchTaskPort
    {
        public Task<ResponseModel<List<TaskEntity>>> ExecuteAsync(SearchTaskModel model, Guid UserId)
        {
            throw new NotImplementedException();
        }
    }
}
