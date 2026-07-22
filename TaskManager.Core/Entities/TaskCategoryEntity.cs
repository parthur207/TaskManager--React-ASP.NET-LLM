using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskManager.Core.Entities
{
    public sealed class TaskCategoryEntity
    {
        public TaskCategoryEntity() { }

        public TaskCategoryEntity(Guid ownerId, Guid spaceId, string name)
        {
            Id = Guid.NewGuid();
            OwnerId = ownerId;
            Name = name;
            CreatedAt = DateTime.Now;
            Tasks = new List<TaskEntity>();
            SpaceId = spaceId;
        }//Criar


        [JsonInclude]
        public Guid Id { get; private set; }

        [JsonInclude]
        public string Name { get; private set; }

        [JsonInclude]
        public DateTime CreatedAt { get; private set; }

        [JsonInclude]
        public DateTime? UpdatedAt { get; private set; }

        [JsonInclude]
        public Guid OwnerId { get; private set; }

        [JsonInclude]
        public UserEntity? UserOwner { get; private set; }

        [JsonInclude]
        public IList<TaskEntity> Tasks { get; private set; }

        [JsonInclude]
        public Guid SpaceId { get; private set; }

        [JsonInclude]
        public SpaceEntity? Space { get; private set; }

        public void UpdateCategoryName(string newName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                throw new ArgumentNullException("O novo nome para a categoria não pode ser nulo.");
            }
            Name = newName;
            UpdatedAt = DateTime.Now;
        }

        public void AssigneTask(TaskEntity newTask)
        {
            if (newTask is null)
            {
                throw new ArgumentNullException(nameof(newTask), "Erro. Tarefa nula.");
            }
            if (Tasks.Contains(newTask))
            {
                throw new InvalidOperationException("A tarefa já está atribuída a esta categoria.");
            }
            Tasks.Add(newTask);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
 