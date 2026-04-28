using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.Space.Interfaces;

namespace TaskManager.Core.UseCases.Space
{
    internal class UpdateNameSpaceUseCase : IUpdateNameSpaceUseCase
    {
        public Task<SimpleResponseModel> ExecuteAsync(Guid SpaceId)
        {
            throw new NotImplementedException();
        }
    }
}
