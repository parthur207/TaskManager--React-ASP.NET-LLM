using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Models;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.Ports.User
{
    public interface ILoginUserPort
    {

        Task<ResponseModel<string>> ExecuteAsync(LoginRequestModel model);
    }
}
