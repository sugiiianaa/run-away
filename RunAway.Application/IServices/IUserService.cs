using RunAway.Application.Features.Users.Commands.CreateUser;
using RunAway.Domain.Entities;

namespace RunAway.Application.IServices
{
    public interface IUserService
    {
        public Task<UserEntity> CreateUserAsync(CreateUserCommand command, CancellationToken cancellationToken);
        public Task<UserEntity?> GetUserEntityByEmailAsync(string email);
    }
}
