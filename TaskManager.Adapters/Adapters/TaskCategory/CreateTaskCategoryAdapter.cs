using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Enums;
using TaskManager.Core.Mappers;
using TaskManager.Core.Models.TaskCategory;
using TaskManager.Core.Ports.Persistence.TaskCategory;
using TaskManager.Core.ResponsePattern;

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
                Debug.Assert(false, "Erro:" + ex.Message);
                throw new Exception("Ocorreu um erro inesperado.");
            }
        }
    }
}
