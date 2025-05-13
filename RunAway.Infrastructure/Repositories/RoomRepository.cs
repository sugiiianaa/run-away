using Microsoft.EntityFrameworkCore;
using RunAway.Application.IRepositories;
using RunAway.Domain.Entities;
using RunAway.Infrastructure.Persistence;

namespace RunAway.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;

        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(IEnumerable<RoomEntity> rooms)
        {
            await _context.Rooms.AddRangeAsync(rooms);
        }

        public async Task<RoomEntity?> GetRoomByIDAsync(Guid roomID)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.ID == roomID);
        }
    }
}
