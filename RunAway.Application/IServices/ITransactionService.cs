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
            string currency,
            decimal unitPrice,
            int numberOfUnits,
            int numberOfDays,
            decimal discount,
            decimal fee,
            IList<Guest> Guests,
            DateOnly CheckInDate,
            DateOnly CheckOutDate,
            CancellationToken cancellationToken);
    }
}
