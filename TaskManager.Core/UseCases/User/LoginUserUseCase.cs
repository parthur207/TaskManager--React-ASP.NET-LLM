using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;
using TaskManager.Core.Models;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.Ports.User;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.User.Interfaces;

namespace TaskManager.Core.UseCases.User
{
    public class LoginUserUseCase : ILoginUserUseCase
    {
        private readonly ILoginUserPort _loginUserPort;
        private readonly IPasswordHasher _passwordHasher;
        public LoginUserUseCase(ILoginUserPort loginUserPort, IPasswordHasher passwordHasher)
        {
            _loginUserPort = loginUserPort;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResponseModel<string>> ExecuteAsync(CreateUserModel model)
        {
            var Response = new ResponseModel<string>();

            if (model is null)
            {
                Response.Status= ResponseStatusEnum.Error;
                Response.Message = "O modelo é nulo.";
                return Response;
            }

            var passwordHash = _passwordHasher.Hash(model.Password);

        }
    }
}
