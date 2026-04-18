using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;

namespace TaskManager.Core.Entities
{
    public class TaskChildrenEntity
    {
        public TaskChildrenEntity(string title, string? description, DateOnly term, Guid parentTaskId)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            StatusEnum = TaskStatusEnum.NotStarted;
            Term = term;
            ParentTaskId = parentTaskId;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ? Description { get; set; }
        public TaskStatusEnum StatusEnum { get; set; }
        public DateOnly Term { get; set; }
        public Guid ParentTaskId { get; set; }
        public TaskEntity ParentTask { get; set; }

 

    }
}
