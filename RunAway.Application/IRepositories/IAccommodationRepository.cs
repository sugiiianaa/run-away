using RunAway.Domain.Entities;

namespace RunAway.Application.IRepositories
{
    public interface IAccommodationRepository
    {
        Task<AccommodationEntity> GetByIdAsync(Guid id);
        Task<AccommodationEntity> GetByIdWithRoomsAsync(Guid id);
        Task AddAsync(AccommodationEntity entity);
        Task UpdateAsync(AccommodationEntity entity);
        Task DeleteAsync(Guid id);
    }
}
