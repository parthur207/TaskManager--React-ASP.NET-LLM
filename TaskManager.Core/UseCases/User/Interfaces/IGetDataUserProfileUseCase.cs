using TaskManager.Core.DTOs;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.UseCases.User.Interfaces
{
    public interface IGetDataUserProfileUseCase
    {
        Task<ResponseModel<UserProfileDTO>> ExecuteAsync();
    }
}
