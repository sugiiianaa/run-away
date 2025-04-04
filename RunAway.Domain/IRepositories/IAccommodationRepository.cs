using RunAway.Domain.Entities;

namespace RunAway.Domain.IRepositories
{
    public interface IAccommodationRepository
    {
        Task AddAsync(AccommodationEntity entity);
        Task UpdateAsync(AccommodationEntity entity);
        Task DeleteAsync(Guid id);
        Task<AccommodationEntity?> GetByIdAsync(Guid id);
    }
}
