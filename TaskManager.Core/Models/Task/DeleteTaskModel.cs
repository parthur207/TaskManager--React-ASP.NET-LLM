using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Models.Task
{
    public class DeleteTaskModel
    {
        public Guid TaskId { get; set; }
        public Guid SpaceId { get; set; }
    
    }
}