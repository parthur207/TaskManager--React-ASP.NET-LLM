

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Adapters.Adapters.ReadServices;
using TaskManager.Adapters.Adapters.Space;
using TaskManager.Adapters.Adapters.Task;
using TaskManager.Adapters.Adapters.TaskCategory;
using TaskManager.Adapters.Adapters.User;
using TaskManager.Adapters.Adapters.User.TaskManager.Adapters.Persistence.User;
using TaskManager.Adapters.Auth;
using TaskManager.Adapters.ExternalServices.AI;
using TaskManager.Adapters.Persistence;
using TaskManager.Adapters.Security;
using TaskManager.Core.Ports.AI;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.Ports.Persistence.TaskCategory;
using TaskManager.Core.Ports.Persistence.User;
using TaskManager.Core.Ports.ReadServices;
using TaskManager.Core.Ports.Security;

namespace TaskManager.Adapters.DI
{
    public static class DependencyInjection_Adapters
    {
        public static IServiceCollection AddAdapters(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            //Database
            services.AddDbContext<DbContextTaskManager>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            //Security 
            services.AddScoped<ICurrentUserPort, HttpCurrentUserAdapter>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtGeneratorPort, JwtGenerator>();

            //User persistence
            services.AddScoped<ICreateUserPort, CreateUserAdapter>();
            services.AddScoped<ILoginUserPort, LoginUserAdapter>();
            services.AddScoped<IUpdateUserPasswordPort, UpdateUserPasswordAdapter>();
            services.AddScoped<IDeleteUserPort, DeleteUserAdapter>();

            //Task persistence
            services.AddScoped<ICreateTaskPort, CreateTaskAdapter>();
            services.AddScoped<IUpdateTaskDetailsPort, UpdateTaskDetailsAdapter>();
            services.AddScoped<IDeleteTaskPort, DeleteTaskAdapter>();
            services.AddScoped<IGetTaskByIdPort, GetTaskByIdAdapter>();
            services.AddScoped<ISearchTaskPort, SearchTaskAdapter>();

            //TaskCategory persistence
            services.AddScoped<ICreateTaskCategoryPort, CreateTaskCategoryAdapter>();
            services.AddScoped<IUpdateTaskCategoryPort, UpdateTaskCategoryAdapter>();
            services.AddScoped<IDeleteTaskCategoryPort, DeleteTaskCategoryAdapter>();

            //Space persistence 
            services.AddScoped<ICreateSpacePort, CreateSpaceAdapter>();

            //Read services (query)
            services.AddScoped<IUserQueryPort, UserQueryAdapter>();
            services.AddScoped<ISpaceMembershipQueryPort, SpaceMembershipQueryAdapter>();

            //External service
            services.AddHttpClient<IOllamaProviderPort, OllamaProviderAdapter>(client =>
            {
                var baseUrl = configuration["Ollama:BaseUrl"] ?? "http://localhost:11434/";
                client.BaseAddress = new Uri(baseUrl);
            });

            return services;
        }
    }
}