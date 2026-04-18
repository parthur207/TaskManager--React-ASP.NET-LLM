using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Models.Space
{
    public class UpdateSpaceModel
    {
        public Guid Id { get; set; }
        public string newName { get; set; }

    }
}
