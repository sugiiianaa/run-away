using RunAway.Application.IRepositories;
using RunAway.Application.IServices;
using RunAway.Domain.Commons;
using RunAway.Domain.Entities;
using RunAway.Domain.Enums;
using RunAway.Domain.ValueObjects;

namespace RunAway.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionRecordEntity> CreateTransactionAsync(
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
            CancellationToken cancellationToken)
        {
            var transactionEntity = TransactionRecordEntity.Create(
                Guid.NewGuid(),
                currency,
                unitPrice,
                numberOfUnits,
                numberOfDays,
                discount,
                fee,
                RoomId,
                UserId,
                [.. Guests],
                CheckInDate,
                CheckOutDate);

            await _transactionRepository.AddAsync(transactionEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return transactionEntity;
        }

        public async Task<IList<TransactionRecordEntity>> GetTransactionAsync(
            Guid userId,
            int transactionStatus,
            int pageNumber,
            int batchSize)
        {
            return await _transactionRepository.GetAsync(batchSize, pageNumber, transactionStatus, userId);
        }

        public async Task<TransactionRecordEntity> GetTransactionByIDAsync(Guid TransactionID)
        {
            return await _transactionRepository.GetByIDAsync(TransactionID);
        }

        public async Task<int> GetTransactionCountAsync(Guid userId, int transactionStatus)
        {
            return await _transactionRepository.GetTotalCountAsync(transactionStatus, userId);
        }

        public async Task UpdateTransactionStatusAsync(TransactionRecordEntity transaction, TransactionStatus newStatus)
        {
            _transactionRepository.UpdateTransactionStatusAsync(transaction, newStatus);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

