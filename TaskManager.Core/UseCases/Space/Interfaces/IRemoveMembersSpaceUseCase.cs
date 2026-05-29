using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.UseCases.Space.Interfaces
{
    public interface IRemoveMembersSpaceUseCase
    {
        Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, ICollection<string> MembersEmails);
    }
}
