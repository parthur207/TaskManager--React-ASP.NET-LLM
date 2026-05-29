using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities;
using TaskManager.Core.Models.Space;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.Ports.Persistence.Space
{
    public interface ICreateSpacePort
    {
        Task<SimpleResponseModel> ExecuteAsync(SpaceEntity entity, Guid UserId);
    }
}
