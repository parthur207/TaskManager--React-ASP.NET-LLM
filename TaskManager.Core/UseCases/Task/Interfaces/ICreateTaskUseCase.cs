using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Models;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.UseCases.Task.Interfaces
{
    public interface ICreateTaskUseCase
    {

        Task<SimpleResponseModel> ExecuteAsync(CreateTaskModel model);
    }
}
