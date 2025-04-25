using RunAway.Domain.Entities;
using RunAway.Domain.ValueObjects;

namespace RunAway.Application.IRepositories
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetByIdAsync(Guid id);
        Task<UserEntity?> GetByEmailAsync(Email email);
        Task AddAsync(UserEntity entity);
        Task UpdateAsync(UserEntity entity);
        Task DeleteAsync(Guid id);
    }
}
