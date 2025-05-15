using RunAway.Application.Commons;
using RunAway.Application.Features.Accommodations.Commands.CreateAccommodations;
using RunAway.Domain.Entities;

namespace RunAway.Application.IServices
{
    public interface IAccommodationService
    {
        public Task<Result<AccommodationEntity>> CreateAccommodationAsync(CreateAccommodationCommand command, CancellationToken cancellationToken);

        public Task<Result<AccommodationEntity>> GetAccommodationDetailAsync(Guid accommodationId, CancellationToken cancellationToken);
    }
}
