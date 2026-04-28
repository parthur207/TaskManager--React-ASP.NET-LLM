using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.UseCases.Space.Interfaces
{
    public interface IUpdateNameSpaceUseCase
    {
        Task<SimpleResponseModel> ExecuteAsync(Guid SpaceId);
    }
}
