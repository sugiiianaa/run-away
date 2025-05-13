using Microsoft.EntityFrameworkCore;
using RunAway.Application.IRepositories;
using RunAway.Domain.Entities;
using RunAway.Infrastructure.Persistence;

namespace RunAway.Infrastructure.Repositories
{
    public class AccommodationAvailabilityRepository : IAccommodationAvailabilityRepository
    {
        private readonly AppDbContext _context;

        public AccommodationAvailabilityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddRoomAvailability(IList<RoomAvailableRecordEntity> roomaAavailableRecordEntities)
        {
            await _context.AddRangeAsync(roomaAavailableRecordEntities);
        }

        public async Task<RoomAvailableRecordEntity?> GetAvailabilityOnSpesificDateAsync(Guid roomID, DateOnly date)
        {
            return await _context.RoomAvailableRecords.FirstOrDefaultAsync(ra => ra.RoomId == roomID && ra.Date == date);
        }
    }
}
