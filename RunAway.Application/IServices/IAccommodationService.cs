using RunAway.Application.Features.Accommodations.Commands.CreateAccommodations;
using RunAway.Application.Features.Accommodations.Queries.GetAccommodationDetails;
using RunAway.Domain.Entities;

namespace RunAway.Application.IServices
{
    public interface IAccommodationService
    {
        public Task<AccommodationEntity> CreateAccommodationAsync(CreateAccommodationCommand command, CancellationToken cancellationToken);

        public Task<AccommodationEntity?> GetAccommodationDetailAsync(GetAccommodationDetailsQuery query, CancellationToken cancellationToken);
    }
}
