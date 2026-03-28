using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Auth;
using TaskManager.Adapters.Mappers;
using TaskManager.Core.Enums;
using TaskManager.Core.Models;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.Ports.User;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Persistence.User
{
    public class LoginUserAdapter : ILoginUserPort
    {
        private readonly DbContextTaskManager _context;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IPasswordHasher _passwordHasher;
        public LoginUserAdapter(DbContextTaskManager context, IJwtGenerator jwtGenerator, IPasswordHasher passwordHasher)
        {
            _context = context;
            _jwtGenerator = jwtGenerator;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResponseModel<string>> ExecuteAsync(LoginRequestModel model)
        {
            var Response= new ResponseModel<string>();
            try
            {
                model.Password = _passwordHasher.Hash(model.Password);

                var mapped = UserMapper.LoginModelToEntity(model);

                if (mapped is null || await _context.User
                    .AnyAsync(x => x.Email.Value != mapped.Email.Value))
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Erro. Login inválido.";
                    return Response;
                }

                var dataUser = await _context.User
                     .FirstOrDefaultAsync(x => x.Email.Value.Equals(mapped.Email.Value)
                     && x.PasswordHash.Value.Equals(mapped.PasswordHash.Value));

                if(dataUser == null || dataUser?.Status!=UserStatusEnum.Active)
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Erro. Login inválido.";
                    return Response;
                }

                Response.Status = ResponseStatusEnum.Success;
                Response.Content = _jwtGenerator.GenerateToken(dataUser.Id, dataUser.Role.ToString());
                Response.Message = "Login efetuado com sucesso.";
            }
            catch (Exception ex)
            {

            }
            return Response;
        }
    }
}
