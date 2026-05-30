using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TaskManager.Core.Ports.Notifications;

namespace TaskManager.API.Hubs
{
    /// <summary>
    /// Hub SignalR responsável apenas por gerenciar conexões e grupos.
    /// Toda lógica de "o que notificar" fica no SignalRSpaceNotifier via IHubContext.
    /// Herda de Hub&lt;ITaskHubClient&gt; para obter chamadas fortemente tipadas.
    /// </summary>
    [Authorize]
    public class TaskHub : Hub<ITaskHubClient>
    {
        /// <summary>
        /// Cliente chama este método após conectar para entrar no grupo do space.
        /// </summary>
        public async Task JoinSpace(string spaceId)
            => await Groups.AddToGroupAsync(Context.ConnectionId, $"space-{spaceId}");

        /// <summary>
        /// Cliente chama este método para sair do grupo do space.
        /// </summary>
        public async Task LeaveSpace(string spaceId)
            => await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"space-{spaceId}");
    }
}