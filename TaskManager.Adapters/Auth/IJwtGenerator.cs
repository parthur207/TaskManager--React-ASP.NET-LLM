using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Adapters.Auth
{
    public interface IJwtGenerator
    {
        string GenerateToken(int UserId, string Role);
    }
}
