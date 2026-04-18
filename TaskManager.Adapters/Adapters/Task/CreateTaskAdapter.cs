using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Mappers;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.Task;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.Ports.Task;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Persistence.Task
{
    public class CreateTaskAdapter : ICreateTaskPort
    {
        private readonly DbContextTaskManager _context;
        private readonly ICurrentUserPort _currentUserPort;
        public CreateTaskAdapter(DbContextTaskManager context, ICurrentUserPort currentUserPort)
        {
            _context = context;
            _currentUserPort = currentUserPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(CreateTaskModel model)
        {
            var Response = new SimpleResponseModel();
            try
            {
                
                var mapped= TaskMapper.ModelToEntity(model, _currentUserPort.UserId);

                if (mapped is null)
                {
                    Response.Status= ResponseStatusEnum.Error;
                    Response.Message="Ocorreu um erro ao mapear os dados da tarefa.";
                    return Response;
                }

                
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro inesperado: {ex.Message}");
            }
            return Response;
        }
    }
}
