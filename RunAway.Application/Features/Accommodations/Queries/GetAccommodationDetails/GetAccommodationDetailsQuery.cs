using MediatR;
using RunAway.Application.Dtos.Accommodation;
using RunAway.Application.IServices;

namespace RunAway.Application.Features.Accommodations.Queries.GetAccommodationDetails
{
    public class GetAccommodationDetailsQuery : IRequest<GetAccomodationDetailResponseDto?>
    {
        public Guid Id { get; set; }
    }

    public class GetAccommodationDetailsQueryHandler : IRequestHandler<GetAccommodationDetailsQuery, GetAccomodationDetailResponseDto?>
    {

        private readonly IAccommodationService _accommodationSerrvice;

        public GetAccommodationDetailsQueryHandler(IAccommodationService accommodationSerrvice)
        {
            _accommodationSerrvice = accommodationSerrvice;
        }

        public async Task<GetAccomodationDetailResponseDto?> Handle(GetAccommodationDetailsQuery request, CancellationToken cancellationToken)
        {
            var accommodation = await _accommodationSerrvice.GetAccommodationDetailAsync(request.Id, cancellationToken);

            if (accommodation == null)
                return null;

            return GetAccommodationDetailMapper.ToGetAccommodationDetailMapperDto(accommodation);
        }
    }
}
