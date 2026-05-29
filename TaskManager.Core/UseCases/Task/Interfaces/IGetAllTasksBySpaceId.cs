using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.UseCases.Task.Interfaces
{
    public interface IGetAllTasksBySpaceId
    {
        Task<ResponseModel<List<TaskDTO>>> ExecuteAsync(Guid spaceId);
    }
}
