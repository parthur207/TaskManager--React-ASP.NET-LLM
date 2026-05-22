using TaskManager.Core.DTOs;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.UseCases.User.Interfaces
{
    public interface IGetDataUserProfileUseCase
    {
        Task<ResponseModel<UserProfileDTO>> ExecuteAsync();
    }
}
