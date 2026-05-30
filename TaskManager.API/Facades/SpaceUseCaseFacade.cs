using TaskManager.Core.UseCases.Space.Interfaces;

namespace TaskManager.API.Facades
{
    public class SpaceUseCaseFacade
    {
        public SpaceUseCaseFacade(ICreateSpaceUseCase create, IDeleteSpaceUseCase delete, IGetUsersBySpaceIdUseCase getUsersBySpaceId, 
            IGetSpaceDetailsByIdUseCase getSpaceById, IGetSpaceDataSideBarUseCase getSpaceDataSideBar)
        {
            this.create = create;
            this.delete = delete;
            this.getUsersBySpaceId = getUsersBySpaceId;
            this.getSpaceById = getSpaceById;
            this.getSpaceDataSideBar = getSpaceDataSideBar;
        }

        public ICreateSpaceUseCase create { get; set; }
        public IDeleteSpaceUseCase delete { get; set; }
        public IGetUsersBySpaceIdUseCase getUsersBySpaceId { get; set; }
        public IGetSpaceDetailsByIdUseCase getSpaceById { get; set; } //infos de um space específico
        public IGetSpaceDataSideBarUseCase getSpaceDataSideBar { get; set; } //infos para o sidebar
        public IAddMembersSpaceUseCase addMembers { get; set; }
        public IRemoveMembersSpaceUseCase removeMembers { get; set; }
        public ILeaveSpaceUseCase leave { get; set; }
        public IUpdateSpaceUseCase updateName { get; set; }

    }
}
