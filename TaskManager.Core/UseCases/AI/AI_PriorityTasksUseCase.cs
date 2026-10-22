using TaskManager.Core.DTOs;
using TaskManager.Core.Enums;
using TaskManager.Core.Mappers;
using TaskManager.Core.Ports.AI;
using TaskManager.Core.Ports.Notifications;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.Prompts;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.AI.Interfaces;

namespace TaskManager.Core.UseCases.AI
{
    public class AI_PriorityTasksUseCase : IAI_PriorityTasksUseCase
    {
        private const int MaxTasksPerRequest = 50; // contexto do modelo é finito

        private readonly IGetAllTasksBySpaceIdPort _getAllTasksBySpaceIdPort;
        private readonly IOllamaProviderPort _ollamaProviderPort;
        private readonly TaskPriorityPrompt _taskPriorityPrompt;
        private readonly ICurrentUserPort _currentUserPort;
        private readonly IAiNotifier _aiNotifier;

        public AI_PriorityTasksUseCase(
            IGetAllTasksBySpaceIdPort getAllTasksBySpaceIdPort,
            IOllamaProviderPort ollamaProviderPort,
            TaskPriorityPrompt taskPriorityPrompt,
            ICurrentUserPort currentUserPort,
            IAiNotifier aiNotifier)
        {
            _getAllTasksBySpaceIdPort = getAllTasksBySpaceIdPort;
            _ollamaProviderPort = ollamaProviderPort;
            _taskPriorityPrompt = taskPriorityPrompt;
            _currentUserPort = currentUserPort;
            _aiNotifier = aiNotifier;
        }

        public async Task<ResponseModel<IEnumerable<AI_PriorityTasksDTO>>> ExecuteAsync(Guid spaceId)
        {
            var response = new ResponseModel<IEnumerable<AI_PriorityTasksDTO>>();

            if (!_currentUserPort.IsAuthenticated)
            {
                response.Status = ResponseStatusEnum.Unauthorized;
                response.Message = "Sessão expirada. Realize o login novamente.";
                return response;
            }

            var repository = await _getAllTasksBySpaceIdPort.ExecuteAsync(spaceId);

            if (repository.Status != ResponseStatusEnum.Success || repository.Content is null)
            {
                response.Status = repository.Status == ResponseStatusEnum.Success ? ResponseStatusEnum.Error : repository.Status;
                response.Message = repository.Message;
                return response;
            }

            var tasks = TaskMapper.ListEntityToListDTO(repository.Content)
                .Where(t => t.Status != nameof(TaskStatusEnum.Finished))
                .OrderBy(t => t.Term)
                .Take(MaxTasksPerRequest)
                .ToList();

            if (tasks.Count == 0)
            {
                response.Content = Enumerable.Empty<AI_PriorityTasksDTO>();
                response.Status = ResponseStatusEnum.Success;
                response.Message = "Nenhuma tarefa pendente para priorizar.";
                return response;
            }

            await _aiNotifier.NotifyJobStarted(spaceId, AiJobKind.Priority, spaceId);

            var ai = await _ollamaProviderPort
                .GenerateAsync<AI_PriorityTasksResponseDTO>(_taskPriorityPrompt.PromptBuilder(tasks));

            if (ai.Status != ResponseStatusEnum.Success || ai.Content?.PrioritizedTasks is null)
            {
                await _aiNotifier.NotifyJobFailed(spaceId, AiJobKind.Priority, spaceId, "Não foi possível priorizar as tarefas agora.");
                response.Status = ai.Status == ResponseStatusEnum.Success ? ResponseStatusEnum.Error : ai.Status;
                response.Message = ai.Message ?? "A IA não retornou uma priorização válida.";
                return response;
            }

            // guarda contra alucinação: só ids que enviamos, sem duplicados, posições renormalizadas 1..n
            var validIds = tasks.Select(t => t.Id).ToHashSet();

            var result = ai.Content.PrioritizedTasks
                .Where(x => validIds.Contains(x.Id))
                .GroupBy(x => x.Id)
                .Select(g => g.First())
                .OrderBy(x => x.PriorityPosition)
                .Select((x, i) => new AI_PriorityTasksDTO
                {
                    TaskId = x.Id,
                    PrioritySort = i + 1,
                    Justify = x.Reason ?? string.Empty
                })
                .ToList();

            var ranked = result.Select(x => x.TaskId).ToHashSet();
            foreach (var missing in tasks.Where(t => !ranked.Contains(t.Id)))
            {
                result.Add(new AI_PriorityTasksDTO
                {
                    TaskId = missing.Id,
                    PrioritySort = result.Count + 1,
                    Justify = "Não classificada pela IA; posicionada ao final."
                });
            }

            // TODO: persistir via IAssignPrioritySortPort (a porta existe, falta o adapter + registro no DI)

            await _aiNotifier.NotifyPriorityReady(spaceId, result);

            response.Content = result;
            response.Status = ResponseStatusEnum.Success;
            return response;
        }
    }
}