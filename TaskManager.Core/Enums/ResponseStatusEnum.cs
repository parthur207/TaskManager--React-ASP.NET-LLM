using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Enums
{
    public enum ResponseStatusEnum
    {
        Success=1,
        Error=2,
        NotFound=3,
        CriticalError=4
    }
}
