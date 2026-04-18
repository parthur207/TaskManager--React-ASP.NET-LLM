using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.User;
using TaskManager.Core.Ports.Persistence.User;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.User.Interfaces;

namespace TaskManager.Core.UseCases.User
{
    public class LoginUserUseCase : ILoginUserUseCase
    {
        private readonly ILoginUserPort _loginUserPort;
        public LoginUserUseCase(ILoginUserPort loginUserPort)
        {
            _loginUserPort = loginUserPort;
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

            var responseRepository = await _loginUserPort.ExecuteAsync(model);
            
            if (responseRepository.Status!=ResponseStatusEnum.Success)
            {
                Response.Status = ResponseStatusEnum.Error;
                Response.Message = "Erro. Login inválido.";
                return Response;
            }
    
            Response.Message = "Login efetuado com sucesso.";
            Response.Content = responseRepository.Content; 
            Response.Status = ResponseStatusEnum.Success;
            
            return Response;
        }
    }
}
