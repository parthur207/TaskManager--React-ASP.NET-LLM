using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;
using TaskManager.Core.Models;
using TaskManager.Core.Ports.User;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.User.Interfaces;

namespace TaskManager.Core.UseCases.User
{
    internal class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly ICreateUserPort _createUserPort;
        
        public CreateUserUseCase(ICreateUserPort createUserPort)
        {
            _createUserPort = createUserPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(CreateUserModel model)
        {
            var Response = new SimpleResponseModel();
            
            if (model is null)
            {
                Response.Message = "Erro. Modelo de criação nula.";
                Response.Status = ResponseStatusEnum.Error;
                return Response;
            }

            var resultRepository= await _createUserPort.ExecuteAsync(model);

            if (resultRepository.Status!=ResponseStatusEnum.Success)
            {
                Response.Status=resultRepository.Status;
                Response.Message = resultRepository.Message;
                return Response;
            }

            return Response=resultRepository;
        }
    }
}
