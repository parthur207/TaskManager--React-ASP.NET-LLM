using Azure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Mappers;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.User;
using TaskManager.Core.Ports.Persistence.User;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Persistence.User
{
    public class CreateUserAdapter : ICreateUserPort
    {
        private readonly DbContextTaskManager _context;
        public CreateUserAdapter(DbContextTaskManager context)
        {
           _context = context;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(CreateUserModel model)
        {
            var Response = new SimpleResponseModel();
            try
            {
                var entityMapper = UserMapper.ModelToEntity(model);

                if (entityMapper is null)
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Erro. Falha no mapeamento da entidade.";
                    return Response;
                }

                if (await _context.User.AnyAsync(
                    x => x.Email.Value == entityMapper.Email.Value))
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Erro. Já existe um usuário com este email.";
                    return Response;
                }

                await _context.User.AddAsync(entityMapper);
                await _context.SaveChangesAsync();

                Response.Status=ResponseStatusEnum.Success;
                Response.Message = "Cadastro realizado com sucesso.";
            }
            catch (Exception ex)
            {
                Response.Status = ResponseStatusEnum.CriticalError;
                Response.Message = $"Erro crítico ao criar usuário: {ex.Message}";
            }
            return Response;
        }
    }
}
