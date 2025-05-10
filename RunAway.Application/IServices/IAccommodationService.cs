using RunAway.Application.Features.Accommodations.Commands.CreateAccommodations;
using RunAway.Domain.Entities;

namespace RunAway.Application.IServices
{
    public interface IAccommodationService
    {
        public Task<AccommodationEntity> CreateAccommodationAsync(CreateAccommodationCommand command, CancellationToken cancellationToken);
    }
}
