using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;

namespace TaskManager.Core.Ports.Notifications
{
    public interface ISpaceNotifier
    {
        Task NotifyTaskCreated(Guid spaceId, TaskDTO task);
        Task NotifyTaskUpdated(Guid spaceId, TaskDTO task);
        Task NotifyTaskDeleted(Guid spaceId, Guid taskId);
        Task NotifySpaceUpdated(Guid spaceId);
    }
}
