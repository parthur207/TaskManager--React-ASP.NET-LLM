using TaskManager.Core.Enums;
using TaskManager.Core.Models.TaskCategory;
using TaskManager.Core.Ports.Persistence.TaskCategory;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.TaskCategory.Interfaces;

namespace TaskManager.Core.UseCases.TaskCategory
{
    internal class CreateTaskCategoryUseCase : ICreateTaskCategoryUseCase
    {
        private readonly ICreateTaskCategoryPort _createTaskCategoryPort;
        public CreateTaskCategoryUseCase(ICreateTaskCategoryPort createTaskCategoryPort)
        {
            _createTaskCategoryPort = createTaskCategoryPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(CreateTaskCategoryModel model)
        {
            var Response= new SimpleResponseModel();

            if (model == null)
            {
                Response.Status=ResponseStatusEnum.Error;
                Response.Message = "O modelo não pode ser nulo";
                return Response;
            }

            var responseRepository= await _createTaskCategoryPort.ExecuteAsync(model);

            if (responseRepository.Status!=ResponseStatusEnum.Success)
            {
                return responseRepository;
            }

            return responseRepository;
        }
    }
}
