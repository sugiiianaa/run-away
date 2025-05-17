using Microsoft.EntityFrameworkCore;
using RunAway.Application.IRepositories;
using RunAway.Domain.Entities;
using RunAway.Domain.Enums;
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

        public async Task<IList<TransactionRecordEntity>> GetAsync(int batchSize, int pageNumber, int transactionStatus, Guid userID)
        {
            // Start with a query for the user's transactions
            var query = _context.Transactions.Where(t => t.UserID == userID);

            // Handle transaction status filtering
            if (transactionStatus != -1)
            {
                if (Enum.IsDefined(typeof(TransactionStatus), transactionStatus))
                {
                    var status = (TransactionStatus)transactionStatus;
                    query = query.Where(t => t.TransactionStatus == status);

                }
            }

            // Apply pagination
            return await query
                .Skip((pageNumber - 1) * batchSize)
                .Take(batchSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync(int transactionStatus, Guid userID)
        {
            var query = _context.Transactions.Where(t => t.UserID == userID);

            if (transactionStatus != -1)
            {
                if (Enum.IsDefined(typeof(TransactionStatus), transactionStatus))
                {
                    var status = (TransactionStatus)transactionStatus;
                    query = query.Where(t => t.TransactionStatus == status);
                }
            }

            return await query.CountAsync();
        }

    }
}
