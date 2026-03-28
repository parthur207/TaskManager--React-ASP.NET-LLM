using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities;

namespace TaskManager.Core.Ports.User
{
    public interface IUserQueryPort
    {
        Task<UserEntity?> GetByEmailAsync(string email);
    }
}
