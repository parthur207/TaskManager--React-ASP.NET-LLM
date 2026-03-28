using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.UseCases.User.Interfaces
{
    public interface IDeleteUserUseCase
    {
        Task<SimpleResponseModel> ExecuteAsync();
    }
}
