using TaskManager.Core.UseCases.User.Interfaces;

namespace TaskManager.API.Facades
{
    public class UserUseCaseFacade
    {
        public UserUseCaseFacade(
            ICreateUserUseCase create,
            IUpdateUserPasswordUseCase updatePassword,
            IDeleteUserUseCase delete,
            ILoginUserUseCase login,
            IGetDataUserProfileUseCase getDatasProfile)
        {
            Create = create;
            UpdatePassword = updatePassword;
            Delete = delete;
            Login = login;
            GetDatasProfile = getDatasProfile;
        }

        public ICreateUserUseCase Create { get; }
        public IUpdateUserPasswordUseCase UpdatePassword { get; }
        public IDeleteUserUseCase Delete { get; }
        public ILoginUserUseCase Login { get; }
        public IGetDataUserProfileUseCase GetDatasProfile { get; }
    }
}