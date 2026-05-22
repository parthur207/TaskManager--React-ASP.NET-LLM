using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.DTOs
{
    public class UserProfileDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<SpaceItemDTO>? MySpaces { get; set; }
        public IEnumerable<SpaceItemDTO>? SpacesMember { get; set; }
    }
}
