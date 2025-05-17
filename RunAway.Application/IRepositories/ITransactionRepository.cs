using RunAway.Domain.Entities;

namespace RunAway.Application.IRepositories
{
    public interface ITransactionRepository
    {
        Task AddAsync(TransactionRecordEntity transactionRecord);
        Task<IList<TransactionRecordEntity>> GetAsync(int batchSize, int pageNumber, int transactionStatus, Guid userID);
        Task<int> GetTotalCountAsync(int transactionStatus, Guid userID);
    }
}
