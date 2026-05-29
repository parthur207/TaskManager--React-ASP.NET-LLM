using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Models.Space
{
    public class MembersRemovedModel
    {
        public Guid SpaceId { get; set; }
        public List<string> MembersEmails { get; set; }
    }
}
