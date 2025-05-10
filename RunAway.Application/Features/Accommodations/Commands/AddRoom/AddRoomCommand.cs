using MediatR;
using RunAway.Application.Dtos.Room;
using RunAway.Application.IServices;

namespace RunAway.Application.Features.Accommodations.Commands.AddRoom
{
    public class AddRoomCommand : IRequest<IList<CreateRoomResponseDto>>
    {
        public Guid AccommodationId { get; set; }
        public required IList<CreateRoomRequestDto> Rooms { get; set; }
    }


    public class AddRoomCommandHandler : IRequestHandler<AddRoomCommand, IList<CreateRoomResponseDto>>
    {
        private readonly IRoomService _roomService;


        public AddRoomCommandHandler(IRoomService roomService)
        {
            _roomService = roomService;
        }

        public async Task<IList<CreateRoomResponseDto>> Handle(AddRoomCommand command, CancellationToken cancellationToken)
        {
            var rooms = await _roomService.AddRoomAsync(command, cancellationToken);

            return CreateRoomMapper.ToCreateRoomResponseDto(rooms);
        }

    }
}
