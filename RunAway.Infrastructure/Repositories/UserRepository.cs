using Microsoft.EntityFrameworkCore;
using RunAway.Application.IRepositories;
using RunAway.Domain.Entities;
using RunAway.Domain.ValueObjects;
using RunAway.Infrastructure.Persistence;

namespace RunAway.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserEntity entity)
        {
            await _context.Users.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
        }

        public async Task<UserEntity?> GetByEmailAsync(Email email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.Value == email.Value);
        }

        public async Task<UserEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public Task UpdateAsync(UserEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
