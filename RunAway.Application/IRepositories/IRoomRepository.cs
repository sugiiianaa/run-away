using RunAway.Domain.Entities;

namespace RunAway.Application.IRepositories
{
    public interface IRoomRepository
    {
        Task AddAsync(IEnumerable<RoomEntity> room);
    }
}
