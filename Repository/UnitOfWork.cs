using Microsoft.EntityFrameworkCore;

namespace IOB___Controle_de_Saldo_Bancario.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContextBank _dbContext;

        public UnitOfWork(DbContextBank dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _dbContext.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }
    }
}
