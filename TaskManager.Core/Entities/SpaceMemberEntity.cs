using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskManager.Core.Entities
{
    public class SpaceMemberEntity
    {
        public SpaceMemberEntity() { }

        [JsonInclude]
        public Guid SpaceId { get; private set; }

        [JsonInclude]
        public SpaceEntity Space { get; private set; }

        [JsonInclude]
        public Guid UserId { get; private set; }

        [JsonInclude]
        public UserEntity User { get; private set; }

        [JsonInclude]
        public DateTime JoinedAt { get; private set; }

        [JsonInclude]
        public bool IsAdmin { get; private set; }

        public SpaceMemberEntity(Guid spaceId, Guid userId, bool isAdmin = false)
        {
            SpaceId = spaceId;
            UserId = userId;
            JoinedAt = DateTime.UtcNow;
            IsAdmin = isAdmin;
        }
    }
}

