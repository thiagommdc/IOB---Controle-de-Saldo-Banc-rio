using IOB___Controle_de_Saldo_Bancario.Model;
using Microsoft.EntityFrameworkCore;

namespace IOB___Controle_de_Saldo_Bancario.Repository
{
    public class BankAccountRepository : IRepository<BankAccount>
    {
        private readonly DbContextBank _context;

        public BankAccountRepository(DbContextBank context)
        {
            _context = context;
        }

        public async Task<BankAccount> GetByIdAsync(int id)
        {
            return await _context.BankAccount.FindAsync(id);
        }

        public async Task AddAsync(BankAccount bankAccount)
        {
            await _context.BankAccount.AddAsync(bankAccount);
        }

        public async Task UpdateAsync(BankAccount bankAccount)
        {
            _context.Entry(bankAccount).State = EntityState.Modified;
        }

        public async Task DeleteAsync(BankAccount bankAccount)
        {
            _context.BankAccount.Remove(bankAccount);
        }

        public Task<List<BankAccount>> GetAllAsync(int id = 0)
        {
            return _context.BankAccount.ToListAsync();
        }
    }
}
