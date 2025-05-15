using MediatR;
using RunAway.Application.Commons;
using RunAway.Application.Dtos.Room;
using RunAway.Application.IServices;

namespace RunAway.Application.Features.Accommodations.Commands.AddRoom
{
    public class AddRoomCommand : IRequest<Result<IList<CreateRoomResponseDto>>>
    {
        public Guid AccommodationId { get; set; }
        public required IList<CreateRoomRequestDto> Rooms { get; set; }
    }


    public class AddRoomCommandHandler : IRequestHandler<AddRoomCommand, Result<IList<CreateRoomResponseDto>>>
    {
        private readonly IRoomService _roomService;


        public AddRoomCommandHandler(IRoomService roomService)
        {
            _roomService = roomService;
        }

        public async Task<Result<IList<CreateRoomResponseDto>>> Handle(AddRoomCommand command, CancellationToken cancellationToken)
        {
            var result = await _roomService.AddRoomAsync(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return Result<IList<CreateRoomResponseDto>>.Failure(result.ErrorMessage, result.ApiResponseErrorCode, result.ErrorCode);
            }

            return Result<IList<CreateRoomResponseDto>>.Success(CreateRoomMapper.ToCreateRoomResponseDto(result.Value!));
        }

    }
}
