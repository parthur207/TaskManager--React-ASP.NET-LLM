using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;

namespace TaskManager.Core.DTOs
{
    public class UserDTO
    {
        public UserDTO(string name, string email, IEnumerable<TaskDTO> tasks, IEnumerable<SpaceMemberDTO> spaces, RoleUserEnum role, UserStatusEnum status)
        {
            Name = name;
            Email = email;
            Tasks = tasks;
            Spaces = spaces;
            Role = role;
            Status = status;
        }

        public string Name { get; set; }
        public string Email { get; set; }   
        public IEnumerable<TaskDTO> Tasks { get; set; }
        public IEnumerable<SpaceMemberDTO> Spaces { get; set; }
        public RoleUserEnum Role { get; set; }
        public UserStatusEnum Status { get; set; }
    }
}
