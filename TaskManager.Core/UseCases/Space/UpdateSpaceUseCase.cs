using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.Space.Interfaces;

namespace TaskManager.Core.UseCases.Space
{
    public class UpdateSpaceUseCase : IUpdateSpaceUseCase
    {
        private readonly IUpdateSpacePort _updateSpacePort;

        public UpdateSpaceUseCase(IUpdateSpacePort updateSpacePort)
        {
            _updateSpacePort = updateSpacePort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, Guid spaceId)
        {
            throw new NotImplementedException();
        }
    }
}
