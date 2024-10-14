using System;

namespace IOB___Controle_de_Saldo_Bancario.Model
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public ICollection<BankLaunch> OriginBankLaunches { get; set; }

        public ICollection<BankLaunch> DestinationBankLaunches { get; set; }
    }
}
