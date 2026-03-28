using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Ports.Space
{
    public interface ISpaceMembershipPort
    {
        Task<bool> IsUserMemberAsync(Guid userId, Guid spaceId);

    }
}
