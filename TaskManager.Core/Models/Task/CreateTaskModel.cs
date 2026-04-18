using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Models.Task
{
    public class CreateTaskModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid SpaceId { get; set; }
        public string? ResponsibleEmail { get; set; }
        public DateOnly Term { get; set; }
    }
}