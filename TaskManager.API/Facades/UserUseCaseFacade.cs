using TaskManager.Core.UseCases.User.Interfaces;

namespace TaskManager.API.Facades
{
    public class UserUseCaseFacade
    {
        public UserUseCaseFacade(ICreateUserUseCase create, IUpdateUserPasswordUseCase updatePassword, IDeleteUserUseCase delete, ILoginUserUseCase login)
        {
            Create = create;
            UpdatePassword = updatePassword;
            Delete = delete;
            Login = login;
        }

        public ICreateUserUseCase Create { get; }
        public IUpdateUserPasswordUseCase UpdatePassword { get; }
        public IDeleteUserUseCase Delete { get; }
        public ILoginUserUseCase Login { get; }
    }
}
