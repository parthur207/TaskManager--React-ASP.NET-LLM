using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Mappers;
using TaskManager.Core.Entities;
using TaskManager.Core.Models;
using TaskManager.Core.Ports.Task;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Persistence.Task
{
    public class CreateTaskAdapter : ICreateTaskPort
    {

        public async Task<SimpleResponseModel> ExecuteAsync(CreateTaskModel model)
        {
            var Response = new SimpleResponseModel();
            try
            {
                var mapped= TaskMapper.ModelToEntity(model);
            }
            catch (Exception ex)
            {

            }
            return Response;
        }
    }
}
