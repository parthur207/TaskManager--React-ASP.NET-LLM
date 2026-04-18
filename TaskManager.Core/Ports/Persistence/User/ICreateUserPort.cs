using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities;
using TaskManager.Core.Models.User;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.Ports.Persistence.User
{
    public interface ICreateUserPort
    {
        Task<SimpleResponseModel> ExecuteAsync(CreateUserModel entity);
    }
}
