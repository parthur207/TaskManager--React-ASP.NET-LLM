using Microsoft.EntityFrameworkCore;
using TaskManager.Adapters.Mappers;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.User;
using TaskManager.Core.Ports.Persistence.User;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Persistence.User
{
    public class LoginUserAdapter : ILoginUserPort
    {
        private readonly DbContextTaskManager _context;
        private readonly IJwtGeneratorPort _jwtGenerator;
        private readonly IPasswordHasher _passwordHasher;
        public LoginUserAdapter(DbContextTaskManager context, IJwtGeneratorPort jwtGenerator, IPasswordHasher passwordHasher)
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
                var mapped = UserMapper.LoginModelToEntity(model);

                if (mapped is null || await _context.User
                    .AnyAsync(x => x.Email.Value != mapped.Email.Value))
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Erro. Login inválido.";
                    return Response;
                }

                var dataUser = await _context.User
                    .Where(x=>x.Status.Equals(UserStatusEnum.Active))
                     .FirstOrDefaultAsync(x => x.Email.Value.Equals(mapped.Email.Value)
                     && _passwordHasher.Verify(model.Password, x.PasswordHash.Value));

                if(dataUser == null || dataUser?.Status!=UserStatusEnum.Active)
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Erro. Login inválido.";
                    return Response;
                }

                Response.Status = ResponseStatusEnum.Success;
                Response.Content = _jwtGenerator.GenerateToken(dataUser.Id, dataUser.Email.Value, dataUser.Role);
                Response.Message = "Login efetuado com sucesso.";
            }
            catch (Exception ex)
            {

            }
            return Response;
        }
    }
}
