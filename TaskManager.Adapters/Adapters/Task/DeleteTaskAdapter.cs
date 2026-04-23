using Microsoft.EntityFrameworkCore;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Adapters.Task
{
    public class DeleteTaskAdapter : IDeleteTaskPort
    {
        private readonly DbContextTaskManager _contextTask;
        public DeleteTaskAdapter(DbContextTaskManager contextTask)
        {
            _contextTask = contextTask;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid IdTask, Guid IdUser)
        {
            var Response= new SimpleResponseModel();
            try
            {
                var entity = await _contextTask.Task
                    .FirstOrDefaultAsync(x => x.Id == IdTask
                    && x.OwnerId == IdUser || x.ResponsibleUserId == IdUser);
                
                if(entity is null)
                {
                    Response.Message = "Tarefa não encontrada, ou sem privilégio de exclusão.";
                    Response.Status= ResponseStatusEnum.Error;
                    return Response;
                }
                
                _contextTask.Task.Remove(entity);
                await _contextTask.SaveChangesAsync();

                Response.Message = "Tarefa excluída com sucesso.";
                Response.Status = ResponseStatusEnum.Success;
                return Response;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro inesperado.");
            }
        }
    }
}
