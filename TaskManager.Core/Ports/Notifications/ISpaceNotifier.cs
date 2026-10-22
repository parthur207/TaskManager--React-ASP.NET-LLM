namespace TaskManager.Core.Ports.Notifications
{

    public interface ISpaceNotifier
    {
        Task NotifySpaceUpdated(Guid spaceId);

        Task NotifySpaceDeleted(Guid spaceId, IEnumerable<Guid> memberIds);

        Task NotifyMembersAdded(Guid spaceId, IEnumerable<Guid> userIds);

        Task NotifyMembersRemoved(Guid spaceId, IEnumerable<Guid> userIds);
    }
}