using MediatR;
using RunAway.Application.Commons;
using RunAway.Application.Dtos.AccommodationAvailability;
using RunAway.Application.IServices;

namespace RunAway.Application.Features.AccommodationAvailabilities.Commands.AddRoomAvailabilities
{
    public class AddRoomAvailabilitiesCommand : IRequest<Result<CreateAccommodationAvailabilityResponseDto?>>
    {
        public required CreateAccommodationAvailabilityRequestDto RoomAvailabilities { get; set; }
    }

    public class AddRoomAvailabilitiesCommandHandler : IRequestHandler<AddRoomAvailabilitiesCommand, Result<CreateAccommodationAvailabilityResponseDto?>>
    {
        private readonly IAccommodationAvailabilityService _accommodationAvailabilityService;

        public AddRoomAvailabilitiesCommandHandler(IAccommodationAvailabilityService accommodationAvailabilityService)
        {
            _accommodationAvailabilityService = accommodationAvailabilityService;
        }

        public async Task<Result<CreateAccommodationAvailabilityResponseDto?>> Handle(AddRoomAvailabilitiesCommand request, CancellationToken cancellationToken)
        {
            // Call the service method which returns Result<IList<RoomAvailableRecordEntity>>
            var result = await _accommodationAvailabilityService.AddRoomAvailabilityAsync(request.RoomAvailabilities);

            // If service operation failed, return the error with same message and code
            if (!result.IsSuccess)
            {
                return Result<CreateAccommodationAvailabilityResponseDto?>.Failure(
                    result.ErrorMessage,
                    result.ApiResponseErrorCode,
                    result.ErrorCode);
            }

            // Map the successful result to response DTO
            var responseDto = CreateAccommodationAvailabilityMapper.ToCreateAccommodationAvailabilityResponseDto(result.Value);

            // Return success with mapped DTO
            return Result<CreateAccommodationAvailabilityResponseDto?>.Success(responseDto);
        }
    }
}
