using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;
using TaskManager.Core.Models;
using TaskManager.Core.Ports.Space;
using TaskManager.Core.Ports.Task;
using TaskManager.Core.Ports.TaskCategory;
using TaskManager.Core.Ports.User;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    public class CreateTaskUseCase : ICreateTaskUseCase
    {
        private readonly ICreateTaskPort _createTaskPort;
        private readonly IUserQueryPort _userQuery;
        private readonly ISpaceMembershipPort _membership;
        public CreateTaskUseCase(ICreateTaskPort createTaskPort, IUserQueryPort userQuery, ISpaceMembershipPort membership)
        {
            _createTaskPort = createTaskPort;
            _userQuery = userQuery;
            _membership = membership;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(CreateTaskModel model)
        {
            var Response = new SimpleResponseModel();

            if (model is null)
            {
                Response.Message = "Erro. Dados nulos.";
                Response.Status = ResponseStatusEnum.Error;
                return Response;
            }

            var DataResponsibleTask= await _userQuery.GetByIdAsync(model.ResponsibleUserId);

            var responseRepository = await _createTaskPort.ExecuteAsync(model);

            if (responseRepository.S)
            {

            }

            return Response;
        }
    }
}
