using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.DTOs
{
    public class SpaceMemberDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int QuantityMembers { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
