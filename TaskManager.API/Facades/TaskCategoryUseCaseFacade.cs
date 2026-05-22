using TaskManager.Core.UseCases.TaskCategory.Interfaces;

namespace TaskManager.API.Facades
{
    public class TaskCategoryUseCaseFacade
    {
        public TaskCategoryUseCaseFacade(ICreateTaskCategoryUseCase create, IUpdateTaskCategoryUseCase update, IDeleteTaskCategoryUseCase delete, IGetAllTaskCategoryUseCase getAll) 
        {
            Create=create; 
            Update=update;
            Delete = delete;
            GetAll = getAll;
        }
        public ICreateTaskCategoryUseCase Create { get; } 
        public IUpdateTaskCategoryUseCase Update { get; }
        public IDeleteTaskCategoryUseCase Delete { get; }
        public IGetAllTaskCategoryUseCase GetAll { get; }
    }
}
