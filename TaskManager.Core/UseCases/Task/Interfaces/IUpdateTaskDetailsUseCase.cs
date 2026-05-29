using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Models.Task;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.UseCases.Task.Interfaces
{
    public interface IUpdateTaskDetailsUseCase
    {
        Task<SimpleResponseModel> ExecuteAsync(UpdateTaskModel model);
    }
}
