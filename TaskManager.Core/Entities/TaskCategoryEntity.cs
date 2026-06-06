using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Entities
{
    public sealed class TaskCategoryEntity
    {
        protected TaskCategoryEntity() { }

        public TaskCategoryEntity(Guid ownerId, Guid spaceId, string name)
        {
            Id = Guid.NewGuid();
            OwnerId = ownerId;
            Name = name;
            CreatedAt = DateTime.Now;
            Tasks = new List<TaskEntity>();
            SpaceId = spaceId;
        }//Criar

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid OwnerId { get; private set; }
        public UserEntity? UserOwner { get; private set; }
        public IList<TaskEntity> Tasks { get; private set; }
        public Guid SpaceId { get; private set; }
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
 