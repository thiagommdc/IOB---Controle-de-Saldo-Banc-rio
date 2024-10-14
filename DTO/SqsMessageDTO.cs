using IOB___Controle_de_Saldo_Bancario.Enum;
using IOB___Controle_de_Saldo_Bancario.Model;

namespace IOB___Controle_de_Saldo_Bancario.DTO
{
    public class SqsMessageDTO
    {
        public OperationTypeEnum TipoTransacao { get; set; }
        public int ContaOrigemId { get; set; }
        public int ContaDestinoId { get; set; }
        public decimal Valor { get; set; }

    }
}
