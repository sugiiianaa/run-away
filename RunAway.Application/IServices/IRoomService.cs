using RunAway.Application.Features.Accommodations.Commands.AddRoom;
using RunAway.Domain.Entities;

namespace RunAway.Application.IServices
{
    public interface IRoomService
    {
        public Task<IList<RoomEntity>> AddRoomAsync(AddRoomCommand command, CancellationToken cancellationToken);
    }
}
