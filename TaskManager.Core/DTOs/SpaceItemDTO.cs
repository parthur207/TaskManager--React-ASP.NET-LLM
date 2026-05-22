using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.DTOs
{
    public class SpaceItemDTO
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int MembersCount { get; set; }
    }
}
