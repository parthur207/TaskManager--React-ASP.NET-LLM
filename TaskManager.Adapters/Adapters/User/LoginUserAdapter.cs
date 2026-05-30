using Microsoft.EntityFrameworkCore;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Enums;
using TaskManager.Core.Mappers;
using TaskManager.Core.Models.User;
using TaskManager.Core.Ports.Persistence.User;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.User
{
    public class LoginUserAdapter : ILoginUserPort
    {
        private readonly DbContextTaskManager _context;
        private readonly IJwtGeneratorPort _jwtGenerator;
        private readonly IPasswordHasher _passwordHasher;

        public LoginUserAdapter(
            DbContextTaskManager context,
            IJwtGeneratorPort jwtGenerator,
            IPasswordHasher passwordHasher)
        {
            _context = context;
            _jwtGenerator = jwtGenerator;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResponseModel<string>> ExecuteAsync(LoginRequestModel model)
        {
            var response = new ResponseModel<string>();
            try
            {
                if (model is null
                    || string.IsNullOrWhiteSpace(model.Email)
                    || string.IsNullOrWhiteSpace(model.Password))
                {
                    response.Status = ResponseStatusEnum.Error;
                    response.Message = "Erro. Login inválido.";
                    return response;
                }

                var dataUser = await _context.User
                    .Where(x => x.Status == UserStatusEnum.Active
                                && x.Email.Value == model.Email.Trim().ToLowerInvariant())
                    .FirstOrDefaultAsync();

                if (dataUser is null)
                {
                    response.Status = ResponseStatusEnum.Error;
                    response.Message = "Erro. Login inválido.";
                    return response;
                }

                if (!_passwordHasher.Verify(model.Password, dataUser.PasswordHash.Value))
                {
                    response.Status = ResponseStatusEnum.Error;
                    response.Message = "Erro. Login inválido.";
                    return response;
                }

                response.Status = ResponseStatusEnum.Success;
                response.Content = _jwtGenerator.GenerateToken(dataUser.Id, dataUser.Email.Value, dataUser.Role);
                response.Message = "Login efetuado com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatusEnum.CriticalError;
                response.Message = $"Erro crítico ao efetuar login: {ex.Message}";
                return response;
            }
        }
    }
}