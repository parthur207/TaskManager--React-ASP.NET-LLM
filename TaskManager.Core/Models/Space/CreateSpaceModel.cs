using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Models.Space
{
    public class CreateSpaceModel
    {
        public string Name { get; set; }
        public List<string>? MembersEmails { get; set; } 
    }
}
