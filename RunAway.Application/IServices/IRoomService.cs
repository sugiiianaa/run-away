using RunAway.Application.Commons;
using RunAway.Application.Features.Accommodations.Commands.AddRoom;
using RunAway.Domain.Entities;

namespace RunAway.Application.IServices
{
    public interface IRoomService
    {
        // TODO : should add check availability service later
        public Task<Result<IList<RoomEntity>?>> AddRoomAsync(AddRoomCommand command, CancellationToken cancellationToken);
        public Task<RoomEntity?> GetRoomByIDAsync(Guid roomID);
    }
}
