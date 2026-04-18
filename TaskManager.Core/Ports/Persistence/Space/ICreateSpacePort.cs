using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Models.Space;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.Ports.Persistence.Space
{
    public interface ICreateSpacePort
    {
        Task<SimpleResponseModel> ExecuteAsync(CreateSpaceModel model, Guid UserId);
    }
}
