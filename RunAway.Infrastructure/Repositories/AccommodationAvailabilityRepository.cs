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

        public async Task AddRoomAvailabilityAsync(IList<RoomAvailableRecordEntity> roomaAavailableRecordEntities)
        {
            await _context.AddRangeAsync(roomaAavailableRecordEntities);
        }

        public async Task<IList<RoomAvailableRecordEntity>?> GetAvailabilitityOnDateRangeAsync(Guid roomID, DateOnly checkInDate, DateOnly checkOutDate, int numberOfRoom)
        {
            var availability = await _context.RoomAvailableRecords
                   .Where(ra => ra.RoomId == roomID &&
                               ra.Date >= checkInDate &&
                               ra.Date < checkOutDate &&
                               ra.AvailableRooms >= numberOfRoom)
                   .ToListAsync();

            return availability;
        }

        public async Task<RoomAvailableRecordEntity?> GetAvailabilityOnSpesificDateAsync(Guid roomID, DateOnly date)
        {
            return await _context.RoomAvailableRecords.FirstOrDefaultAsync(ra => ra.RoomId == roomID && ra.Date == date);
        }
    }
}
