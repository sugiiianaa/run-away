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
        public async Task AddAsync(RoomEntity room)
        {
            await _context.Rooms.AddAsync(room);
        }
    }
}
