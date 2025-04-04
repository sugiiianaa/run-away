using Microsoft.EntityFrameworkCore;
using RunAway.Domain.Entities;
using RunAway.Domain.IRepositories;
using RunAway.Infrastructure.Persistence;

namespace RunAway.Infrastructure.Repositories
{
    public class AccommodationRepository : IAccommodationRepository
    {
        private readonly AppDbContext _context;

        public AccommodationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AccommodationEntity entity)
        {
            await _context.Accommodations.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Accommodations.FindAsync(id);
            if (entity != null)
            {
                _context.Accommodations.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<AccommodationEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Accommodations
                .Include(a => a.Rooms)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAsync(AccommodationEntity entity)
        {
            _context.Accommodations.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
