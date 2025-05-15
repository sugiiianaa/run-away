using MediatR;
using Microsoft.Extensions.Logging;
using RunAway.Application.Commons;
using RunAway.Application.Dtos.Accommodation;
using RunAway.Application.IServices;

namespace RunAway.Application.Features.Accommodations.Commands.CreateAccommodations
{
    public class CreateAccommodationCommand : IRequest<Result<CreateAccommodationResponseDto>>
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public required List<string> ImageUrls { get; set; }
        public required List<CreateAccommodationRoomRequestDto> Rooms { get; set; }
    }

    public class CreateAccommodationCommandHandler : IRequestHandler<CreateAccommodationCommand, Result<CreateAccommodationResponseDto>>
    {
        private readonly ILogger<CreateAccommodationCommandHandler> _logger;
        private readonly IAccommodationService _accommodationService;

        public CreateAccommodationCommandHandler(
            ILogger<CreateAccommodationCommandHandler> logger,
            IAccommodationService accommodationService)
        {
            _logger = logger;
            _accommodationService = accommodationService;
        }

        public async Task<Result<CreateAccommodationResponseDto>> Handle(CreateAccommodationCommand request, CancellationToken cancellationToken)
        {
            var result = await _accommodationService.CreateAccommodationAsync(request, cancellationToken);

            if (!result.IsSuccess)
            {
                return Result<CreateAccommodationResponseDto>.Failure(
                    result.ErrorMessage,
                    result.ApiResponseErrorCode,
                    ErrorCode.UnknownError);
            }

            return Result<CreateAccommodationResponseDto>.Success(CreateAccommodationMapper.ToCreateAccommodationResponseDto(result.Value!));
        }
    }
}
