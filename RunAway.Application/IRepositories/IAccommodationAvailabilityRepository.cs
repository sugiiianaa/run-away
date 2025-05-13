using RunAway.Domain.Entities;

namespace RunAway.Application.IRepositories
{
    public interface IAccommodationAvailabilityRepository
    {
        public Task<RoomAvailableRecordEntity?> GetAvailabilityOnSpesificDateAsync(Guid roomID, DateOnly date);

        public Task AddRoomAvailability(IList<RoomAvailableRecordEntity> roomaAavailableRecordEntities);
    }
}
