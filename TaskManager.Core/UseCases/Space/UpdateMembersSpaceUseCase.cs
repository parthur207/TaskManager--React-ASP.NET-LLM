using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.Space.Interfaces;

namespace TaskManager.Core.UseCases.Space
{
    public class UpdateMembersSpaceUseCase : IUpdateMembersSpaceUseCase
    {
        public Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, IEnumerable<string> memberIds)
        {
            throw new NotImplementedException();
        }
    }
}
