using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TaskManager.Core.Enums;

namespace TaskManager.Core.Entities
{
    public class TaskChildrenEntity
    {
        public TaskChildrenEntity() { }

        public TaskChildrenEntity(string title, string? description, DateOnly term, Guid parentTaskId)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            StatusEnum = TaskStatusEnum.NotStarted;
            Term = term;
            ParentTaskId = parentTaskId;
        }

        [JsonInclude]
        public Guid Id { get; private set; }

        [JsonInclude]
        public string Title { get; set; }

        [JsonInclude]
        public string ? Description { get; set; }

        [JsonInclude]
        public TaskStatusEnum StatusEnum { get; set; }

        [JsonInclude]
        public DateOnly Term { get; set; }

        [JsonInclude]
        public Guid ParentTaskId { get; set; }

        [JsonInclude]
        public TaskEntity ParentTask { get; set; }
    }
}
