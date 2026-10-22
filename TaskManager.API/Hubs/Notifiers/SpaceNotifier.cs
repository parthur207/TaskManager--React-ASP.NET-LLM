using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;
using TaskManager.API.Hubs.RealTime;
using TaskManager.API.Realtime;
using TaskManager.Core.Ports.Notifications;

namespace TaskManager.API.Hubs.Notifiers
{
    public sealed class SpaceNotifier : ISpaceNotifier
    {
        private readonly IHubContext<TaskManagerHub, ITaskManagerHubClient> _hub;
        private readonly IUserConnectionTracker _tracker;
        private readonly ILogger<SpaceNotifier> _logger;

        public SpaceNotifier(
            IHubContext<TaskManagerHub, ITaskManagerHubClient> hub,
            IUserConnectionTracker tracker,
            ILogger<SpaceNotifier> logger)
        {
            _hub = hub;
            _tracker = tracker;
            _logger = logger;
        }

        public Task NotifySpaceUpdated(Guid spaceId)
            => HubSafe.RunAsync(_logger, nameof(NotifySpaceUpdated),
                () => _hub.Clients.Group(HubGroups.Space(spaceId)).SpaceUpdated(spaceId));

        public Task NotifySpaceDeleted(Guid spaceId, IEnumerable<Guid> memberIds)
            => HubSafe.RunAsync(_logger, nameof(NotifySpaceDeleted), async () =>
            {
                await _hub.Clients.Group(HubGroups.Space(spaceId)).SpaceDeleted(spaceId);
                await _hub.Clients.Users(ToUserIds(memberIds)).SpaceDeleted(spaceId); // sidebar de quem não está na tela do espaço
            });

        public Task NotifyMembersAdded(Guid spaceId, IEnumerable<Guid> userIds)
            => HubSafe.RunAsync(_logger, nameof(NotifyMembersAdded), async () =>
            {
                await _hub.Clients.Users(ToUserIds(userIds)).AddedToSpace(spaceId);
                await _hub.Clients.Group(HubGroups.Space(spaceId)).SpaceUpdated(spaceId);
            });

        public Task NotifyMembersRemoved(Guid spaceId, IEnumerable<Guid> userIds)
            => HubSafe.RunAsync(_logger, nameof(NotifyMembersRemoved), async () =>
            {
                var ids = userIds.ToList();

                await _hub.Clients.Users(ToUserIds(ids)).RemovedFromSpace(spaceId);

                // tira as conexões do grupo NO SERVIDOR: o cliente removido não recebe mais eventos do espaço
                foreach (var userId in ids)
                    foreach (var connectionId in _tracker.GetConnections(userId))
                        await _hub.Groups.RemoveFromGroupAsync(connectionId, HubGroups.Space(spaceId));

                await _hub.Clients.Group(HubGroups.Space(spaceId)).SpaceUpdated(spaceId);
            });

        private static IReadOnlyList<string> ToUserIds(IEnumerable<Guid> ids)
            => ids.Select(x => x.ToString()).ToList();
    }
}