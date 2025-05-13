using RunAway.Application.Dtos.AccommodationAvailability;
using RunAway.Domain.Entities;

namespace RunAway.Application.IServices
{
    public interface IAccommodationAvailabilityService
    {
        public Task<IList<RoomAvailableRecordEntity>?> AddRoomAvailability(CreateAccommodationAvailabilityRequestDto request);
    }
}
