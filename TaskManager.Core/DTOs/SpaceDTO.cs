using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.DTOs
{
    public class SpaceDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<TaskDTO>? Tasks { get; set; }
        public IEnumerable<TaskCategoryDTO>? TaskCategories { get; set; }
        public IEnumerable<UserMemberSpaceDTO>? Members { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
