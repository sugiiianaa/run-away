using Microsoft.EntityFrameworkCore;
using RunAway.Domain.Commons;

namespace RunAway.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Detect all entities in Added state and ensure their state is correctly set
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    // Ensure added entities have proper state
                    _context.Entry(entry.Entity).State = entry.State;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
