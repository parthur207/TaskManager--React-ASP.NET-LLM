using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.TaskCategory.Interfaces;

namespace TaskManager.Core.UseCases.TaskCategory
{
    public class GetAllTaskCategoryUseCase : IGetAllTaskCategoryUseCase
    {
        private readonly ICurrentUserPort _currentUserPort;

        public GetAllTaskCategoryUseCase(ICurrentUserPort currentUserPort)
        {
            _currentUserPort = currentUserPort;
        }

        public Task<ResponseModel<IEnumerable<TaskCategoryDTO>>> ExecuteAsync(Guid spaceId)
        {
            throw new NotImplementedException();
        }
    }
}
