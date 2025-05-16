using RunAway.Application.IRepositories;
using RunAway.Application.IServices;
using RunAway.Domain.Commons;
using RunAway.Domain.Entities;
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
    }
}

