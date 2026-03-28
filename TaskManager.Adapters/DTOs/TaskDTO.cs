using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;

namespace TaskManager.Adapters.DTOs
{
    public sealed class TaskDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? TaskCategoryName { get; set; }
        public TaskStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateOnly Term { get; set; }
        public UserDTO? User { get; set; }
    }
}
