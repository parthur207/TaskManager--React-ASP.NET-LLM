using TaskManager.Core.UseCases.Space.Interfaces;

namespace TaskManager.API.Facades
{
    public class SpaceUseCaseFacade
    {

        public ICreateSpaceUseCase create { get; set; }
        public IUpdateMembersSpaceUseCase update { get; set; }
        public IUpdateNameSpaceUseCase updateNameSpace { get; set; }
        public IDeleteSpaceUseCase delete { get; set; }
        public IGetSpacesUseCase getSpaces { get; set; }7

    }
}
