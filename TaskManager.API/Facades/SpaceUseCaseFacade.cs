using TaskManager.Core.UseCases.Space.Interfaces;

namespace TaskManager.API.Facades
{
    public class SpaceUseCaseFacade
    {

        public ICreateSpaceUseCase create { get; set; }
        public IDeleteSpaceUseCase delete { get; set; }
        public IGetUsersBySpaceIdUseCase getUsersBySpaceId { get; set; }
        public IGetSpaceDetailsByIdUseCase getSpaceById { get; set; }
        public IUpdateMembersSpaceUseCase updateMembers { get; set; }
        public IUpdateNameSpaceUseCase updateNameSpace { get; set; }

    }
}
