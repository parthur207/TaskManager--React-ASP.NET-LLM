using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;

namespace TaskManager.Core.Ports.Notifications
{
    public interface ITaskHubClient
    {
        Task TaskCreated(TaskDTO task);
        Task TaskUpdated(TaskDTO task);
        Task TaskDeleted(Guid taskId);
    }
}
