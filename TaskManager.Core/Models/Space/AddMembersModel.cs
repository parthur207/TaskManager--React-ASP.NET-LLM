using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Models.Space
{
    public class AddMembersModel
    {
        public Guid spaceId { get; set; }
        public ICollection<string> membersEmails { get; set; }
    }
}
