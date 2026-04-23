using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;

namespace TaskManager.Core.Mappers
{
    public class SpaceMemberMapper
    {
        public static SpaceMemberDTO EntityToDTO(SpaceMemberEntity entity)
        {
            return new SpaceMemberDTO
            {
                Id = entity.SpaceId,
                Name = entity.Space.Name,
                QuantityMembers= entity.Space.Members.Count(),
                JoinedAt = entity.JoinedAt
            };
        }
        public static IEnumerable<SpaceMemberDTO> ListEntityToListDTO(IEnumerable<SpaceMemberEntity> entities)
        {
            if (entities is null) return new List<SpaceMemberDTO>();
            return entities.Select(EntityToDTO).ToList();
        }
    }
}
