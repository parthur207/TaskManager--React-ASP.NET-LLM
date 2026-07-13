using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Notifications;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.Space.Interfaces;

namespace TaskManager.Core.UseCases.Space
{
    public class LeaveSpaceUseCase : ILeaveSpaceUseCase
    {
        private readonly ICurrentUserPort _currentUserPort;
        private readonly ILeaveSpacePort _leaveSpacePort;
        private readonly ISignalRNotifier _notifier;
        public LeaveSpaceUseCase(ICurrentUserPort currentUserPort, ILeaveSpacePort leaveSpacePort, ISignalRNotifier notifier)
        {
            _currentUserPort = currentUserPort;
            _leaveSpacePort = leaveSpacePort;
            _notifier = notifier;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid SpaceId)
        {
            var Response = new SimpleResponseModel();
            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Message = "Sessão Expirada. Efetue o login novamente.";
                Response.Status = ResponseStatusEnum.Unauthorized;
                return Response;
            }

            var responseRepository = await _leaveSpacePort.ExecuteAsync(SpaceId, _currentUserPort.UserId);
            
            if(responseRepository.Status==ResponseStatusEnum.Success)
            await _notifier.NotifySpaceUpdated(SpaceId);

            return responseRepository;
        }
    }
}
