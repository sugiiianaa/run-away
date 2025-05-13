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
            Money Price,
            IList<Guest> Guests,
            CancellationToken cancellationToken)
        {
            var transactionPrice = new Money(Price.Amount, Price.Currency);

            var transactionEntity = TransactionRecordEntity.Create(
                Guid.NewGuid(),
                transactionPrice,
                RoomId,
                UserId,
                Guests.ToList());

            await _transactionRepository.AddAsync(transactionEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return transactionEntity;
        }
    }
}

