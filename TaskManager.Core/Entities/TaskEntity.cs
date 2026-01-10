using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;

namespace TaskManager.Core.Entities
{
    public class TaskEntity
    {

        public Guid Id { get; private set; } = new Guid { };
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public Guid CategoryId { get; private set; }
        public TaskCategoryEntity Category { get; private set; }
        public TaskStatusEnum StatusEnum { get; private set; }= TaskStatusEnum.NotStarted;
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime UpdatedAt { get; private set; } 

    }
}
