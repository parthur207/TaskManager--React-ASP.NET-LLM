using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Notifications;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.Space.Interfaces;

namespace TaskManager.Core.UseCases.Space
{
    public class AddMembersSpaceUseCase : IAddMembersSpaceUseCase
    {
        private readonly ICurrentUserPort _currentUserPort;
        private readonly IAddMembersSpacePort _addMembersSpacePort;
        private readonly ISignalRNotifier _notifier;
        public AddMembersSpaceUseCase(ICurrentUserPort currentUserPort, IAddMembersSpacePort addMembersSpacePort, ISignalRNotifier notifier)
        {
            _currentUserPort = currentUserPort;
            _addMembersSpacePort = addMembersSpacePort;
            _notifier = notifier;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, ICollection<string> MembersEmails)
        {
            var Response = new SimpleResponseModel();
            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Message = "Sessão expirada. Efetue o login novamente.";
                Response.Status = ResponseStatusEnum.Unauthorized;
                return Response;
            }

            var responseRepository = await _addMembersSpacePort.ExecuteAsync(spaceId, MembersEmails);
            
            await _notifier.NotifySpaceUpdated(spaceId);

            return responseRepository;
        }
    }
}
