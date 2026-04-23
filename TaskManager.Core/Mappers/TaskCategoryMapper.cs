using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Models.TaskCategory;

namespace TaskManager.Core.Mappers
{
    public class TaskCategoryMapper
    {
        public static TaskCategoryDTO EntityToDTO(TaskCategoryEntity entity)
        {
            return new TaskCategoryDTO()
            {
                Id=entity.Id,
                Name=entity.Name
            };
        }

        public static TaskCategoryEntity ModelToEntity(CreateTaskCategoryModel model, Guid userId)
        {
            return new TaskCategoryEntity(userId, model.Name);
        }

        public static IEnumerable<TaskCategoryDTO> ListEntityToListDTO(IEnumerable<TaskCategoryEntity> entities)
        {
            if (entities is null) return new List<TaskCategoryDTO>();
            return entities.Select(EntityToDTO).ToList();
        }
    }
}
