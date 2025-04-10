using Microsoft.EntityFrameworkCore;
using RunAway.Application.IRepositories;
using RunAway.Domain.Entities;
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
        }

        public async Task DeleteAsync(Guid id)
        {
            var accommodation = await _context.Accommodations.FindAsync(id);
            if (accommodation != null)
            {
                _context.Accommodations.Remove(accommodation);
            }
        }

        public async Task<AccommodationEntity> GetByIdAsync(Guid id)
        {
            return await _context.Accommodations
                .Include(a => a.Rooms)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AccommodationEntity> GetByIdWithRoomsAsync(Guid id)
        {
            return await _context.Accommodations
                .Include(a => a.Rooms)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public Task UpdateAsync(AccommodationEntity entity)
        {
            // EF Core automatically tracks changes to the entity
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
