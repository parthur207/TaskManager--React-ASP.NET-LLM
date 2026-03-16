using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.Ports.User
{
    public interface ICreateUserPort
    {
        Task<SimpleResponseModel> ExecuteAsync(UserEntity entity);
    }
}
