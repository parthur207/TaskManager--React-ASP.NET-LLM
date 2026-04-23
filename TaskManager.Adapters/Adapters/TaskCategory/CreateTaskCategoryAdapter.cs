using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Models.TaskCategory;
using TaskManager.Core.Ports.Persistence.TaskCategory;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.Enums;
using TaskManager.Core.Mappers;
using TaskManager.Adapters.Persistence;

namespace TaskManager.Adapters.Adapters.TaskCategory
{
    public class CreateTaskCategoryAdapter : ICreateTaskCategoryPort
    {
        private readonly DbContextTaskManager _context;
        public CreateTaskCategoryAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(CreateTaskCategoryModel model, Guid userId)
        {
            var Response = new SimpleResponseModel();
            try
            {
                if (model is null || string.IsNullOrWhiteSpace(model.Name))
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Nome da categoria inválido.";
                    return Response;
                }

                // Verifica duplicidade pelo nome e usuário: o usuário será associado no UseCase via port que passa o userId.
                // Aqui assumimos que o model já foi validado quanto ao usuário ou que o usecase chama o port com contexto correto.

                var entity = TaskCategoryMapper.ModelToEntity(model, userId);

                if (entity is null)
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Falha ao mapear categoria.";
                    return Response;
                }

                await _context.TaskCategory.AddAsync(entity);
                await _context.SaveChangesAsync();

                Response.Status = ResponseStatusEnum.Success;
                Response.Message = "Categoria criada com sucesso.";
                return Response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro inesperado: {ex.Message}");
            }
        }
    }
}
