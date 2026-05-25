using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;

namespace TaskManager.Adapters.Adapters.Space
{
    public class UpdateSpaceAdapter
    {

        private readonly DbContextTaskManager _context;

        public UpdateSpaceAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        
    }
}
