using RunAway.Domain.Entities;

namespace RunAway.Application.IRepositories
{
    public interface ITransactionRepository
    {
        Task AddAsync(TransactionRecordEntity transactionRecord);
    }
}
