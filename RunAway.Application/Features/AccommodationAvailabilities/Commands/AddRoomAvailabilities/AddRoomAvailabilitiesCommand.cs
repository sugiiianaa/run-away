using MediatR;
using RunAway.Application.Dtos.AccommodationAvailability;
using RunAway.Application.IServices;

namespace RunAway.Application.Features.AccommodationAvailabilities.Commands.AddRoomAvailabilities
{
    public class AddRoomAvailabilitiesCommand : IRequest<CreateAccommodationAvailabilityResponseDto?>
    {
        public required CreateAccommodationAvailabilityRequestDto RoomAvailabilities { get; set; }
    }

    public class AddRoomAvailabilitiesCommandHandler : IRequestHandler<AddRoomAvailabilitiesCommand, CreateAccommodationAvailabilityResponseDto?>
    {
        private readonly IAccommodationAvailabilityService _accommodationAvailabilityService;

        public AddRoomAvailabilitiesCommandHandler(IAccommodationAvailabilityService accommodationAvailabilityService)
        {
            _accommodationAvailabilityService = accommodationAvailabilityService;
        }

        public async Task<CreateAccommodationAvailabilityResponseDto?> Handle(AddRoomAvailabilitiesCommand request, CancellationToken cancellationToken)
        {
            var roomAvailabilitiesRecord = await _accommodationAvailabilityService.AddRoomAvailability(request.RoomAvailabilities);

            if (roomAvailabilitiesRecord == null)
                return null;

            return CreateAccommodationAvailabilityMapper.ToCreateAccommodationAvailabilityResponseDto(roomAvailabilitiesRecord);
        }
    }
}
