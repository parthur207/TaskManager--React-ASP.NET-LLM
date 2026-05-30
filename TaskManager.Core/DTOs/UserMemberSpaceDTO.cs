using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;

namespace TaskManager.Core.DTOs
{
    public class UserMemberSpaceDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public UserStatusEnum Status { get; set; }
    }
}
