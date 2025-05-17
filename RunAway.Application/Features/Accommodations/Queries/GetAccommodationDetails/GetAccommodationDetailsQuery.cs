using MediatR;
using RunAway.Application.Commons;
using RunAway.Application.Dtos.Accommodation;
using RunAway.Application.IServices;

namespace RunAway.Application.Features.Accommodations.Queries.GetAccommodationDetails
{
    public class GetAccommodationDetailsQuery : IRequest<Result<GetAccomodationDetailResponseDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetAccommodationDetailsQueryHandler(IAccommodationService accommodationSerrvice) : IRequestHandler<GetAccommodationDetailsQuery, Result<GetAccomodationDetailResponseDto>>
    {
        private readonly IAccommodationService _accommodationSerrvice = accommodationSerrvice;

        public async Task<Result<GetAccomodationDetailResponseDto>> Handle(GetAccommodationDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = await _accommodationSerrvice.GetAccommodationDetailAsync(request.Id, cancellationToken);

            if (!result.IsSuccess)
            {
                return Result<GetAccomodationDetailResponseDto>.Failure(result.ErrorMessage, result.ApiResponseErrorCode, result.ErrorCode);
            }

            return Result<GetAccomodationDetailResponseDto>.Success(GetAccommodationDetailMapper.ToGetAccommodationDetailMapperDto(result.Value!));
        }
    }
}
