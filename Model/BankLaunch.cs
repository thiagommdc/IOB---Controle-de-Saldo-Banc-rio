using System;

namespace IOB___Controle_de_Saldo_Bancario.Model
{
    public class BankLaunch
    {
        public int Id { get; set; }
        public int? OriginBankAccountId { get; set; }
        public BankAccount? OriginBankAccount { get; set; }
        public decimal? Value { get; set; }
        public int? OperationType { get; set; }
        public int? DestinationBankAccountId { get; set; }
        public BankAccount? DestinationBankAccount { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
