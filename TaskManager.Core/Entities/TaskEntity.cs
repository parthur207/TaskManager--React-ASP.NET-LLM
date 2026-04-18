using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;

namespace TaskManager.Core.Entities
{
    public sealed class TaskEntity
    {
        public TaskEntity(string title, string? description, 
            Guid? categoryId, Guid spaceId, Guid ownerId, Guid responsibleUserId, DateOnly term)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            CategoryId = categoryId;
            SpaceId = spaceId;
            OwnerId = ownerId;
            ResponsibleUserId = responsibleUserId;
            StatusEnum = TaskStatusEnum.NotStarted;
            Term = term;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public Guid OwnerId { get; set; }
        public UserEntity OwnerUser { get; private set; }
        public Guid? ResponsibleUserId { get; private set; }
        public UserEntity? ResponsibleUser { get; set; }
        public Guid? CategoryId { get; private set; }
        public TaskCategoryEntity? Category { get; private set; }
        public Guid SpaceId { get; private set; }
        public SpaceEntity Space { get;private set; }
        public TaskStatusEnum StatusEnum { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateOnly Term { get; private set; }

        public void UpdateTitleOrDescription(string newTitle = null, string newDescription = null)
        {
            if (string.IsNullOrEmpty(newTitle) &&
                string.IsNullOrEmpty(Description))
            {
                throw new ArgumentNullException("Erro. Nome e descrição nulos.");
            }
            Title = newTitle;
            Description = newDescription;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateStatusTask(TaskStatusEnum newStatus)
        {
            if (StatusEnum.Equals(newStatus))
            {
                throw new ArgumentException($"O status '{newStatus.ToString()}' já se encontra definido.");
            }

            if (StatusEnum != TaskStatusEnum.NotStarted
               && newStatus is TaskStatusEnum.NotStarted)
            {
                throw new ArgumentException($"Não é possível atribuir o status de '{newStatus.ToString()}', pois a tarefa se encontra com.");
            }
            StatusEnum = newStatus;
        }

        public void AssignResponsibleUser(Guid responsibleUserId)
        {
            if (ResponsibleUserId.Equals(responsibleUserId))
            {
                throw new ArgumentException("O usuário já é responsável por esta tarefa.");
            }
            ResponsibleUserId = responsibleUserId;
        }
    }
}
