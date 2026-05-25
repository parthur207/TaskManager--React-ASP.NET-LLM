using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Models.Task
{
    public class UpdateTaskModel
    {
        public Guid Id { get; set; }
        public IDictionary<bool, string> Title { get; set; }
        public IDictionary<bool, string> Description { get; set; }
        public IDictionary<bool, string> ResponsibleUser { get; set; }
        public IDictionary<bool, string> Category { get; set; }
        public IDictionary<bool, string> Status { get; set; }
        public IDictionary<bool, DateTime> Term { get; set; }
    }
}
