using RunAway.Domain.Entities;

namespace RunAway.Application.IRepositories
{
    public interface IAccommodationAvailabilityRepository
    {
        public Task<RoomAvailableRecordEntity?> GetAvailabilityOnSpesificDateAsync(Guid roomID, DateOnly date);

        public Task<IList<RoomAvailableRecordEntity>?> GetAvailabilitityOnDateRangeAsync(Guid roomID, DateOnly checkInDate, DateOnly checkOutDate, int numberOfRoom);

        public Task AddRoomAvailabilityAsync(IList<RoomAvailableRecordEntity> roomaAavailableRecordEntities);
    }
}
