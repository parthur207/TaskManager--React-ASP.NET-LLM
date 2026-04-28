using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.UseCases.Space;
using TaskManager.Core.UseCases.Space.Interfaces;
using TaskManager.Core.UseCases.Task;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.DI
{
    public static class DependencyInjection_Core
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
        {

            services.AddScoped<ICreateSpaceUseCase, CreateSpaceUseCase>();
            services.AddScoped<IDeleteSpaceUseCase, DeleteSpaceUseCase>();
            services.AddScoped<IUpdateMembersSpaceUseCase, UpdateMembersSpaceUseCase>();
            services.AddScoped<IUpdateNameSpaceUseCase, UpdateNameSpaceUseCase>();

            services.AddScoped<ICreateTaskUseCase, CreateTaskUseCase>();
            services.AddScoped<IDeleteTaskUseCase, DeleteTaskUseCase>();
            services.AddScoped<IGetTaskByIdUseCase, GetTaskByIdUseCase>();



            return services;
        }
    }
}
