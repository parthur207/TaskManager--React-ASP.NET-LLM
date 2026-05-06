using TaskManager.Core.DTOs;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.Ports.ReadServices
{
    public interface ISpaceMembershipQueryPort
    {
        Task<ResponseModel<bool>> IsUserMemberAsync(Guid userId, Guid spaceId);
        Task<ResponseModel<IEnumerable<SpaceDTO>>> GetUserSpacesDetailsAsync(Guid userId);
        Task<ResponseModel<IEnumerable<string>>> GetUsersEmailsInSpaceAsync(Guid spaceId, Guid userId);
    }
}
