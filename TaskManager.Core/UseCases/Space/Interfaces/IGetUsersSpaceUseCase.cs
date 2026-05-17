using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.UseCases.Space.Interfaces
{
    public interface IGetUsersBySpaceIdUseCase
    {
        Task<ResponseModel<IEnumerable<string>>> ExecuteAsync(Guid spaceId);
    }
}
