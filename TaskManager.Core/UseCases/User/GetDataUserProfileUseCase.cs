using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
using TaskManager.Core.Enums;
using TaskManager.Core.Mappers;
using TaskManager.Core.Ports.Persistence.User;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.User.Interfaces;

namespace TaskManager.Core.UseCases.User
{
    public class GetDataUserProfileUseCase : IGetDataUserProfileUseCase
    {

        private readonly IGetDataUserProfilePort _getDataUserProfilePort;
        private readonly ICurrentUserPort _currentUserPort;
        public GetDataUserProfileUseCase(IGetDataUserProfilePort getDataUserProfilePort, ICurrentUserPort currentUserPort)
        {
            _getDataUserProfilePort = getDataUserProfilePort;
            _currentUserPort = currentUserPort;
        }

        public async Task<ResponseModel<UserProfileDTO>> ExecuteAsync()
        {
            var Response= new ResponseModel<UserProfileDTO>();
            
            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Message = "";
                Response.Status = ResponseStatusEnum.Unauthorized;
                return Response;
            }

            var ResponseRepository = await _getDataUserProfilePort
                .GetDataUserProfileAsync(_currentUserPort.UserId);

            Response.Content = UserMapper.ListEntityToListProfileDTO(ResponseRepository.Content);
            Response.Status = ResponseRepository.Status;
            Response.Message = ResponseRepository.Message;
            return Response;

        }
    }
}
