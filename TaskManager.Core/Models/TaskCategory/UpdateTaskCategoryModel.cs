using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Models.TaskCategory
{
    public class UpdateTaskCategoryModel
    {
        public Guid TaskCategoryId { get; set; }
        public string newName { get; set; }
    }
}
