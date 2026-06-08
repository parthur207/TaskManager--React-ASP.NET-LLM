using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;

namespace TaskManager.Core.Models.Task
{
    public class SearchTaskModel
    {

        public string? Category { get; set; }
        public TaskStatusEnum? StatusEnum { get; set; }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

    }
}
