using RunAway.Application.Commons;
using RunAway.Application.Dtos.AccommodationAvailability;
using RunAway.Domain.Entities;

namespace RunAway.Application.IServices
{
    public interface IAccommodationAvailabilityService
    {
        public Task<Result<IList<RoomAvailableRecordEntity>>> AddRoomAvailabilityAsync(CreateAccommodationAvailabilityRequestDto request);

        public Task<IList<RoomAvailableRecordEntity>> GetRoomAvailabilityOnDateRangeAsync(
            Guid roomID,
            DateOnly checkInDate,
            DateOnly checkOutDate,
            int numberOfRoom);


        public Task<Result<bool>> UpdateRoomAvailabilityAsync(Guid roomID,
            DateOnly checkInDate,
            DateOnly checkOutDate,
            int numberOfRoomsToBook);
    }
}
