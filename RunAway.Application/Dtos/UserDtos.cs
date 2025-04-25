using RunAway.Domain.Entities;
using RunAway.Domain.ValueObjects;

namespace RunAway.Application.Dtos
{
    // Dtos
    public record UserIdDto(Guid ID);
    public record UserDto(Guid ID, string Email, string Username);

    // Mapper
    public static class UserMapper
    {
        public static UserDto ToUserDto(UserEntity user)
        {
            return new UserDto(user.Id, user.Email, user.Name);
        }

        public static UserIdDto ToUserIdDto(UserEntity user)
        {
            return new UserIdDto(user.Id);
        }

        public static UserEntity ToUserEntity(
            Guid id,
            Email email,
            string password,
            string name)
        {
            return new UserEntity(id, email, password, name);
        }
    }
}
