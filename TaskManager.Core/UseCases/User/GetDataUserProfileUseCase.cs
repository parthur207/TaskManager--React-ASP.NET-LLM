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

        public GetDataUserProfileUseCase(
            IGetDataUserProfilePort getDataUserProfilePort,
            ICurrentUserPort currentUserPort)
        {
            _getDataUserProfilePort = getDataUserProfilePort;
            _currentUserPort = currentUserPort;
        }

        public async Task<ResponseModel<UserProfileDTO>> ExecuteAsync()
        {
            var response = new ResponseModel<UserProfileDTO>();

            if (!_currentUserPort.IsAuthenticated)
            {
                response.Message = "Sessão expirada. Efetue o login novamente.";
                response.Status = ResponseStatusEnum.Unauthorized;
                return response;
            }

            var repositoryResponse = await _getDataUserProfilePort
                .ExecuteAsync(_currentUserPort.UserId);

            if (repositoryResponse.Status != ResponseStatusEnum.Success
                || repositoryResponse.Content is null)
            {
                response.Status = repositoryResponse.Status;
                response.Message = repositoryResponse.Message;
                return response;
            }

            response.Content = UserMapper.EntityToUserProfileDTO(repositoryResponse.Content);
            response.Status = repositoryResponse.Status;
            response.Message = repositoryResponse.Message;
            return response;
        }
    }
}