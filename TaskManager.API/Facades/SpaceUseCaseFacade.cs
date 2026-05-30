using TaskManager.Core.UseCases.Space.Interfaces;

namespace TaskManager.API.Facades
{
    public class SpaceUseCaseFacade
    {
        public SpaceUseCaseFacade(
            ICreateSpaceUseCase create,
            IDeleteSpaceUseCase delete,
            IGetUsersBySpaceIdUseCase getUsersBySpaceId,
            IGetSpaceDetailsByIdUseCase getSpaceById,
            IGetSpaceDataSideBarUseCase getSpaceDataSideBar,
            IAddMembersSpaceUseCase addMembers,
            IRemoveMembersSpaceUseCase removeMembers,
            ILeaveSpaceUseCase leave,
            IUpdateSpaceUseCase updateName)
        {
            this.create = create;
            this.delete = delete;
            this.getUsersBySpaceId = getUsersBySpaceId;
            this.getSpaceById = getSpaceById;
            this.getSpaceDataSideBar = getSpaceDataSideBar;
            this.addMembers = addMembers;
            this.removeMembers = removeMembers;
            this.leave = leave;
            this.updateName = updateName;
        }

        public ICreateSpaceUseCase create { get; }
        public IDeleteSpaceUseCase delete { get; }
        public IGetUsersBySpaceIdUseCase getUsersBySpaceId { get; }
        public IGetSpaceDetailsByIdUseCase getSpaceById { get; }
        public IGetSpaceDataSideBarUseCase getSpaceDataSideBar { get; }
        public IAddMembersSpaceUseCase addMembers { get; }
        public IRemoveMembersSpaceUseCase removeMembers { get; }
        public ILeaveSpaceUseCase leave { get; }
        public IUpdateSpaceUseCase updateName { get; }
    }
}