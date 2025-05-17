using RunAway.Domain.Entities;
using RunAway.Domain.Enums;
using RunAway.Domain.ValueObjects;

namespace RunAway.Application.IServices
{
    public interface ITransactionService
    {
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

        public Task<IList<TransactionRecordEntity>> GetTransactionAsync(
            Guid userId,
            int transactionStatus,
            int pageNumber,
            int batchSize);

        public Task<int> GetTransactionCountAsync(
            Guid userId,
            int transactionStatus);

        public Task<TransactionRecordEntity> GetTransactionByIDAsync(
            Guid TransactionID);

        public Task UpdateTransactionStatusAsync(TransactionRecordEntity transaction, TransactionStatus newStatus);
    }
}
