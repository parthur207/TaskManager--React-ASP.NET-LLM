using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Models.User;

namespace TaskManager.Core.Mappers
{
    public class UserMapper
    {
        public static UserEntity ModelToEntity(CreateUserModel model)
        {
            return new UserEntity
                (
                    model.Name,
                    model.Email,
                    model.Password
                );
        }

        public static UserEntity LoginModelToEntity(LoginRequestModel model)
        {
            return new UserEntity
                (
                    model.Email,
                    model.Password
                );
        }

        public static UserDTO EntityToDTO(UserEntity entity)
        {
            return new UserDTO
                (
                    entity.Name,
                    entity.Email.Value,
                    TaskMapper.ListEntityToListDTO(entity?.Tasks),
                    SpaceMemberMapper.ListEntityToListDTO(entity?.Spaces),
                    entity.Role,
                    entity.Status
                );
        }

        public static IEnumerable<UserDTO> ListEntityToListDTO(IEnumerable<UserEntity> entities)
        {
            if (entities is null) return new List<UserDTO>();
            return entities.Select(EntityToDTO).ToList();
        }
    }
}
