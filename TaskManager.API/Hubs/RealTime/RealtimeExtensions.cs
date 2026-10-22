using System.Text.Json.Serialization;
using TaskManager.API.Hubs.Notifiers;
using TaskManager.API.Realtime.Notifiers;
using TaskManager.Core.Ports.Notifications;

namespace TaskManager.API.Hubs.RealTime
{
    public static class RealtimeExtensions
    {
        public const string HubPath = "/hubs/taskManager";

        public static IServiceCollection AddRealtime(this IServiceCollection services, bool detailedErrors = false)
        {
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = detailedErrors;
                options.KeepAliveInterval = TimeSpan.FromSeconds(15);
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
            })
                .AddJsonProtocol(o => o.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            // Várias instâncias da API? Adicione o backplane (Redis já faz parte da stack):
            //   dotnet add TaskManager.API package Microsoft.AspNetCore.SignalR.StackExchangeRedis
            //   .AddStackExchangeRedis(configuration["Redis:ConnectionString"]!, o => o.Configuration.ChannelPrefix = RedisChannel.Literal("TaskManager"));

            services.AddSingleton<IUserConnectionTracker, UserConnectionTracker>();
            services.AddSingleton<ITaskNotifier, TaskNotifier>();
            services.AddSingleton<ISpaceNotifier, SpaceNotifier>();
            services.AddSingleton<IAiNotifier, AiNotifier>();

            // TRANSITÓRIO: mantém compilando/rodando os use cases que ainda injetam ISignalRNotifier.
            // Remova junto com o ISignalRNotifier quando todos migrarem para ITaskNotifier/ISpaceNotifier.
            services.AddSingleton<ISignalRNotifier, LegacySignalRNotifier>();
            return services;
        }

        public static IEndpointRouteBuilder MapRealtime(this IEndpointRouteBuilder app)
        {
            app.MapHub<TaskManagerHub>(HubPath);
            return app;
        }
    }
}