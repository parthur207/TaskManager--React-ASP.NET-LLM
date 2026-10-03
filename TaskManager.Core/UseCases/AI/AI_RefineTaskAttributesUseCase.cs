using TaskManager.Core.DTOs;
using TaskManager.Core.Enums;
using TaskManager.Core.Mappers;
using TaskManager.Core.Ports.AI;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.Prompts;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.AI.Interfaces;

namespace TaskManager.Core.UseCases.AI
{
    public class AI_RefineTaskAttributesUseCase : IAI_RefineTaskAttributesUseCase
    {
        private readonly IGetTaskByIdPort _getTaskByIdPort;
        private readonly IOllamaProviderPort _ollamaProviderPort;
        private readonly ICurrentUserPort _currentUserPort;
        private readonly RefineTaskAttributesPrompt _refineTaskAttributesPrompt;
        public AI_RefineTaskAttributesUseCase(IGetTaskByIdPort getTaskByIdPort, IOllamaProviderPort ollamaProviderPort,
            ICurrentUserPort currentUserPort, RefineTaskAttributesPrompt refineTaskAttributesPrompt)
        {
            _getTaskByIdPort = getTaskByIdPort;
            _ollamaProviderPort = ollamaProviderPort;
            _currentUserPort = currentUserPort;
            _refineTaskAttributesPrompt = refineTaskAttributesPrompt;
        }

        public async Task<ResponseModel<IEnumerable<TaskDTO>>> ExecuteAsync(Guid taskId)
        {
            var Response = new ResponseModel<IEnumerable<TaskDTO>>();

            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Status = ResponseStatusEnum.Unauthorized;
                Response.Message = "Sessão expirada. Realize o login novamente.";
                return Response;
            }

            var responseRepository = await _getTaskByIdPort.ExecuteAsync(taskId, _currentUserPort.UserId);

            if (responseRepository.Status != ResponseStatusEnum.Success)
            {
                Response.Message = responseRepository.Message;
                Response.Status = responseRepository.Status;
                return Response;
            }
            var prompt = _refineTaskAttributesPrompt
                .PromptBuilder(TaskMapper.EntityToDTO(responseRepository.Content));

            var ResponseIA = await _ollamaProviderPort.GenerateAsync<ResponseModel<IEnumerable<TaskDTO>>>(prompt);

            if (ResponseIA.Status!=ResponseStatusEnum.Success)
            {
                Response.Status = ResponseIA.Status;
                Response.Message = ResponseIA.Message;
                return Response;
            }

            Response.Status = ResponseStatusEnum.Success;
            Response.Content = ResponseIA.Content as IEnumerable<TaskDTO> ?? Enumerable.Empty<TaskDTO>();
            Response.Message= ResponseIA.Message;

            return Response;
        }
    }
}
