using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;

namespace TaskManager.Core.ResposePattern
{
    public class SimpleResponseModel
    {
        public SimpleResponseModel() { }

        public ResponseStatusEnum Status { get; set; }

        public string? Message { get; set; } 
    }
}
