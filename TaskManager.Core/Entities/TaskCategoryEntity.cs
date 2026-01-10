using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Entities
{
    public class TaskCategoryEntity
    {

        public Guid Id { get; private set; }
        //Fk
        public Guid UserId { get; private set; }
        public 
        public string Name { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get;
    }
}
