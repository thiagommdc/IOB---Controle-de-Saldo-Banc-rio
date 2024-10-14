namespace IOB___Controle_de_Saldo_Bancario.DTO
{
    public class TransferenciaDto
    {
        public int ContaOrigemId { get; set; }
        public int ContaDestinoId { get; set; }
        public decimal Valor { get; set; }
    }
}
