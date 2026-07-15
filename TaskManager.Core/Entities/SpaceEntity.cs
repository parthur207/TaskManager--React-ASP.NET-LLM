using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TaskManager.Core.Entities
{
    public class SpaceEntity
    {
        public SpaceEntity() { } 

        public SpaceEntity(string name, DateTime createdAt)
        { 
            Name=name;
            CreatedAt=createdAt;
            Members?.Count();
        }
            
        public SpaceEntity(string name, Guid ownerId, IList<SpaceMemberEntity>? members)
        {
            Id = Guid.NewGuid();
            Tasks = [];
            Name = name;
            OwnerId = ownerId;
            Members = members?.ToList() ?? new List<SpaceMemberEntity>();
            TaskCategories = [];
            Members.Add(new SpaceMemberEntity(Id, ownerId));
            CreatedAt= DateTime.Now;
        }

        public Guid Id { get; private set; }
        public IList<TaskEntity>? Tasks { get; set; }
        public string Name { get;set; }
        public Guid OwnerId { get; set; } 
        public UserEntity Owner { get; set; }
        public IList<SpaceMemberEntity>? Members { get; set; }
        public IList<TaskCategoryEntity>? TaskCategories { get; set; }
        public DateTime CreatedAt { get; private set; } 
        public DateTime? UpdatedAt { get; private set; }

        public void AddMember(UserEntity user)
        {
            if (Members.Any(x => x.UserId == user.Id))
                throw new InvalidOperationException("Usuário já pertence ao espaço.");

            Members.Add(new SpaceMemberEntity(Id, user.Id));
            UpdatedAt= DateTime.UtcNow;
        }

        public void RemoveMember(Guid userId)
        {
            var member = Members?.FirstOrDefault(x => x.UserId == userId);

            if (member is null)
                throw new InvalidOperationException("Usuário não pertence ao espaço.");

            Members?.Remove(member);
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeName(string newName)
        {
            if (string.IsNullOrEmpty(newName))
                throw new ArgumentNullException(nameof(newName), "O nome do espaço não pode ser nulo ou vazio.");

            if(newName.Length > 60)
               throw new ArgumentNullException(nameof(newName), "O novo nome para o espaço não pode ter mais que 60 caracteres.");

            Name = newName;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
