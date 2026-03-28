using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Models;

namespace TaskManager.Adapters.Mappers
{
    public class TaskMapper
    {
        public TaskEntity ModelToEntity(
            CreateTaskModel model,
            Guid userId)
        {
            return new TaskEntity(
                model.Name,
                model.Description,
                model.CategoryId,
                model.SpaceId,
                userId,
                model.Term
            );
        }

        public TaskDTO EntityToDTO(TaskEntity entity)
        {
            return new TaskDTO(
                entity.Name,
                Description: entity.Description,
                CategoryId: entity.CategoryId,
                StatusEnum: entity.StatusEnum
            );
        }
    }
}
