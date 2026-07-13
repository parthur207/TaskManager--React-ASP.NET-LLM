using TaskManager.Core.Models.Task;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.UseCases.Task.Interfaces
{
    public interface IDeleteTaskUseCase
    {
        Task<SimpleResponseModel> ExecuteAsync(DeleteTaskModel model);
    }
}