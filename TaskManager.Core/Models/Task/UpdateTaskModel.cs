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
        public string Title { get; set; }
    }
}
