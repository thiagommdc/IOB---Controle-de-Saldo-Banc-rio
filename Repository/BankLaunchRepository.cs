using IOB___Controle_de_Saldo_Bancario.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IOB___Controle_de_Saldo_Bancario.Repository
{
    public class BankLaunchRepository : IRepository<BankLaunch>
    {
        private readonly DbContextBank _context;

        public BankLaunchRepository(DbContextBank context)
        {
            _context = context;
        }

        public async Task<BankLaunch> GetByIdAsync(int id)
        {
            return await _context.BankLaunch.FindAsync(id);
        }

        public async Task AddAsync(BankLaunch bankLaunch)
        {
            await _context.BankLaunch.AddAsync(bankLaunch);
        }

        public async Task UpdateAsync(BankLaunch bankLaunch)
        {
            _context.Entry(bankLaunch).State = EntityState.Modified;
        }

        public async Task DeleteAsync(BankLaunch bankLaunch)
        {
            _context.BankLaunch.Remove(bankLaunch);
        }
       
        public async Task<List<BankLaunch>> GetAllAsync(int accountId)
        {
            return await _context.BankLaunch
                .Where(l => l.OriginBankAccountId == accountId || l.DestinationBankAccountId == accountId)
                .Include(l => l.OriginBankAccount) 
                .Include(l => l.DestinationBankAccount)
                .ToListAsync();
        }
    }
}
