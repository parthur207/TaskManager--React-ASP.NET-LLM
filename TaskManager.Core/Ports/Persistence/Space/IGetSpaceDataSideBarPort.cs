using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.Ports.Persistence.Space
{
    public interface IGetSpaceDataSideBarPort
    {
        Task<ResponseModel<IEnumerable<SpaceEntity>>> ExecuteAsync(Guid userId);
    }
}
