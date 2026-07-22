using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.Ports.Persistence.Space
{
    public interface IAddMembersSpacePort
    {
        Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, Guid userId, ICollection<string> Members);        
    }
}
