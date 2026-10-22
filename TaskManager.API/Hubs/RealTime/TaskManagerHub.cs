using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TaskManager.API.Realtime;
using TaskManager.Core.Ports.Notifications;
using TaskManager.Core.Ports.ReadServices;

namespace TaskManager.API.Hubs.RealTime
{
    [Authorize]
    public sealed class TaskManagerHub : Hub<ITaskManagerHubClient>
    {
        private readonly ISpaceMembershipQueryPort _membership;
        private readonly IUserConnectionTracker _tracker;

        public TaskManagerHub(ISpaceMembershipQueryPort membership, IUserConnectionTracker tracker)
        {
            _membership = membership;
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            if (TryGetUserId(out var userId))
                _tracker.Add(userId, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (TryGetUserId(out var userId))
                _tracker.Remove(userId, Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>O cliente só entra no grupo se for membro do espaço (antes qualquer usuário logado entrava).</summary>
        public async Task JoinSpace(Guid spaceId)
        {
            if (!TryGetUserId(out var userId))
                throw new HubException("Sessão inválida.");

            var isMember = await _membership.IsUserMemberAsync(userId, spaceId);
            if (!isMember.Content)
                throw new HubException("Você não é membro deste espaço.");

            await Groups.AddToGroupAsync(Context.ConnectionId, HubGroups.Space(spaceId));
        }

        public Task LeaveSpace(Guid spaceId)
            => Groups.RemoveFromGroupAsync(Context.ConnectionId, HubGroups.Space(spaceId));

        // UserIdentifier = claim NameIdentifier (mesma que o JwtGenerator emite).
        private bool TryGetUserId(out Guid userId)
            => Guid.TryParse(Context.UserIdentifier, out userId);
    }
}