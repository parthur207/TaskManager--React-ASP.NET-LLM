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

        public async Task<ResponseModel<string>> ExecuteAsync(LoginRequestModel model)
        {
            var Response = new ResponseModel<string>();
            
            if (model is null)
            {
                Response.Status = ResponseStatusEnum.Error;
                Response.Message = "O modelo é nulo.";
                return Response;
            }

            _passwordHasher.Hash(model.Password);

            var responseRepository = await _loginUserPort.ExecuteAsync(model);
            
            if (responseRepository.Status!=ResponseStatusEnum.Success)
            {
                Response.Status = ResponseStatusEnum.Error;
                Response.Message = "Erro. Login inválido.";
                return Response;
                }
    
                    if (!_passwordHasher.Verify(model.Password, user.PasswordHash))
                    {
                        Response.Status = ResponseStatusEnum.Error;
                        Response.Message = "Senha incorreta.";
                        return Response;
                    }
                    Response.Message = "Login efetuado com sucesso.";
                    Response.Content = "TokenDeAutenticacao"; 
                    Response.Status = ResponseStatusEnum.Success;
            }
            catch (Exception ex)
            {

            }
            return Response;
        }
    }
}
