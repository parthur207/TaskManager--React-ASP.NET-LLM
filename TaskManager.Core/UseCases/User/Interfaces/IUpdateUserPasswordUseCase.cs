using TaskManager.Core.Models.User;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.UseCases.User.Interfaces
{
    public interface IUpdateUserPasswordUseCase
    {
        Task<SimpleResponseModel> ExecuteAsync(UpdateUserPasswordModel model);
    }
}