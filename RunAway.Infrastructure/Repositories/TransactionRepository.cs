using RunAway.Application.IRepositories;
using RunAway.Domain.Entities;
using RunAway.Infrastructure.Persistence;

namespace RunAway.Infrastructure.Repositories
{
    public class TransactionRepository(AppDbContext context) : ITransactionRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddAsync(TransactionRecordEntity transactionRecord)
        {
            await _context.Transactions.AddAsync(transactionRecord);
        }
    }
}
