using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Ports.Persistence.User;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.User.Interfaces;

namespace TaskManager.Core.UseCases.User
{
    internal class DeleteUserUseCase : IDeleteUserUseCase
    {

        private readonly IDeleteUserPort _deleteUserPort;
        public DeleteUserUseCase(IDeleteUserPort deleteUserPort)
        {
            _deleteUserPort = deleteUserPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync()
        {
            
        }
    }
}
