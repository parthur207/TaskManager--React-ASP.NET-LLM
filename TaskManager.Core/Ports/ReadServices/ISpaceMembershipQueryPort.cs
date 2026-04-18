using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.DTOs;

namespace TaskManager.Core.Ports.ReadServices
{
    public interface ISpaceMembershipQueryPort
    {
        Task<bool> IsUserMemberAsync(Guid userId, Guid spaceId);

        Task<IEnumerable<Guid>> GetUserSpacesAsync(Guid userId);
        Task<IEnumerable<SpaceDTO>> GetUserSpacesDetailsAsync(Guid userId);
    }
}
