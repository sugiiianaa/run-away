using RunAway.Domain.Entities;
using RunAway.Domain.ValueObjects;

namespace RunAway.Application.IServices
{
    public interface ITransactionService
    {
        // should add check availability service later
        public Task<TransactionRecordEntity> CreateTransactionAsync(
            Guid RoomId,
            Guid UserId,
            Money Price,
            IList<Guest> Guests,
            DateOnly CheckInDate,
            DateOnly CheckOutDate,
            CancellationToken cancellationToken = default);
    }
}
