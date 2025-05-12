using RunAway.Application.Features.Users.Commands.CreateUser;
using RunAway.Domain.Entities;
using RunAway.Domain.ValueObjects;

namespace RunAway.Application.Dtos.User
{

    // Response
    public class CreateUserResponseDto
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
    }

    // Mapper
    public static class CreateUserMapper
    {
        public static UserEntity ToUserEntity(CreateUserCommand command)
        {
            return UserEntity.Create(
                Guid.NewGuid(),
                new Email(command.Email),
                command.Password,
                command.Name,
                Domain.Enums.UserRoles.User);
        }
    }
}
