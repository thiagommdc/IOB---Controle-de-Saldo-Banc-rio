using IOB___Controle_de_Saldo_Bancario.Model;
using Microsoft.EntityFrameworkCore;

namespace IOB___Controle_de_Saldo_Bancario.Repository
{
    public class DbContextBank : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContextBank(DbContextOptions<DbContextBank> options)
           : base(options)
        {
        }

        public DbSet<BankAccount> BankAccount { get; set; }
        public DbSet<BankLaunch> BankLaunch { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankLaunch>()
                .HasOne(bl => bl.OriginBankAccount)
                .WithMany(ba => ba.OriginBankLaunches)
                .HasForeignKey(bl => bl.OriginBankAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BankLaunch>()
                .HasOne(bl => bl.DestinationBankAccount)
                .WithMany(ba => ba.DestinationBankLaunches)
                .HasForeignKey(bl => bl.DestinationBankAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
