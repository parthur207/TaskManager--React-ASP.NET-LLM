using TaskManager.Core.DTOs;
using TaskManager.Core.Enums;
using TaskManager.Core.Mappers;
using TaskManager.Core.Ports.AI;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.Prompts;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.AI.Interfaces;

namespace TaskManager.Core.UseCases.AI
{
    public class AI_PriorityTasksUseCase : IAI_PriorityTasksUseCase
    {
        private readonly IGetAllTasksBySpaceIdPort _getAllTasksBySpaceIdPort;
        private readonly IOllamaProviderPort _ollamaProviderPort;
        private readonly TaskPriorityPrompt _taskPriorityPrompt;
        private readonly ICurrentUserPort _currentUserPort;

        public AI_PriorityTasksUseCase(IGetAllTasksBySpaceIdPort getAllTasksBySpaceIdPort, IOllamaProviderPort ollamaProviderPort, 
            TaskPriorityPrompt taskPriorityPrompt, ICurrentUserPort currentUserPort)
        {
            _getAllTasksBySpaceIdPort = getAllTasksBySpaceIdPort;
            _ollamaProviderPort = ollamaProviderPort;
            _taskPriorityPrompt = taskPriorityPrompt;
            _currentUserPort = currentUserPort;
        }
        public async Task<ResponseModel<IEnumerable<AI_PriorityTasksDTO>>> ExecuteAsync(Guid spaceId)
        {
            var Response = new ResponseModel<IEnumerable<AI_PriorityTasksDTO>>();

            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Status = ResponseStatusEnum.Unauthorized;
                Response.Message = "Sessão expirada. Realize o login novamente.";
                return Response;
            }

            var responseRepository = await _getAllTasksBySpaceIdPort.ExecuteAsync(spaceId);

            if (responseRepository.Status != ResponseStatusEnum.Success)
            {
                Response.Message = responseRepository.Message;
                Response.Status = responseRepository.Status;
                return Response;
            }

        

            var ResponseIA = await _ollamaProviderPort
                .GenerateAsync<IEnumerable<AI_PriorityTasksDTO>>(_taskPriorityPrompt
                .PromptBuilder(TaskMapper.ListEntityToListDTO(responseRepository.Content)));

            if (ResponseIA.Status != ResponseStatusEnum.Success)
            {
                Response.Status = ResponseIA.Status;
                Response.Message = ResponseIA.Message;
                return Response;
            }

            Response.Status = ResponseStatusEnum.Success;
            Response.Content = ResponseIA.Content as IEnumerable<AI_PriorityTasksDTO> ?? Enumerable.Empty<AI_PriorityTasksDTO>();
            return Response;
        }
    }
}
