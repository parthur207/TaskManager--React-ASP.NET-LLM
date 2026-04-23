using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Models.Space;

namespace TaskManager.Core.Mappers
{
    public class SpaceMapper
    {
        public static SpaceEntity ModelToEntity(
            CreateSpaceModel model,
            Guid ownerId,
            IList<SpaceMemberEntity>? resolvedMembers = null)
        {
            return new SpaceEntity(model.Name, ownerId, resolvedMembers);
        }

        public static SpaceDTO EntityToDTO(SpaceEntity entity)
        {
            return new SpaceDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public static IEnumerable<SpaceDTO> ListEntityToListDTO(IEnumerable<SpaceEntity> entities)
        {
            if (entities is null) return new List<SpaceDTO>();
            return entities.Select(EntityToDTO).ToList();
        }
    }
}