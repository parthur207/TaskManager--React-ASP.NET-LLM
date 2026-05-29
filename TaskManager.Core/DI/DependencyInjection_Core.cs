using Microsoft.Extensions.DependencyInjection;
using TaskManager.Core.UseCases.Space;
using TaskManager.Core.UseCases.Space.Interfaces;
using TaskManager.Core.UseCases.Task;
using TaskManager.Core.UseCases.Task.Interfaces;
using TaskManager.Core.UseCases.TaskCategory;
using TaskManager.Core.UseCases.TaskCategory.Interfaces;
using TaskManager.Core.UseCases.User;
using TaskManager.Core.UseCases.User.Interfaces;

namespace TaskManager.Core.DI
{
    public static class DependencyInjection_Core
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            //Space 
            services.AddScoped<ICreateSpaceUseCase, CreateSpaceUseCase>();
            services.AddScoped<IDeleteSpaceUseCase, DeleteSpaceUseCase>();
            services.AddScoped<IAddMembersSpaceUseCase, AddMembersSpaceUseCase>();
            services.AddScoped<IRemoveMembersSpaceUseCase, RemoveMembersSpaceUseCase>();

            //Task
            services.AddScoped<ICreateTaskUseCase, CreateTaskUseCase>();
            services.AddScoped<IUpdateTaskDetailsUseCase, UpdateTaskDetailsUseCase>();
            services.AddScoped<IDeleteTaskUseCase, DeleteTaskUseCase>();
            services.AddScoped<IGetTaskByIdUseCase, GetTaskByIdUseCase>();
            services.AddScoped<ISearchTaskUseCase, SearchTaskUseCase>();
            services.AddScoped<IGetTasksBySpaceUseCase, GetTasksBySpace>();

            //TaskCategory
            services.AddScoped<ICreateTaskCategoryUseCase, CreateTaskCategoryUseCase>();
            services.AddScoped<IUpdateTaskCategoryUseCase, UpdateTaskCategoryUseCase>();
            services.AddScoped<IDeleteTaskCategoryUseCase, DeleteTaskCategoryUseCase>();

            //User
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
            services.AddScoped<ILoginUserUseCase, LoginUserUseCase>();
            services.AddScoped<IUpdateUserPasswordUseCase, UpdateUserPasswordUseCase>();
            services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();

            return services;
        }
    }
}