namespace IOB___Controle_de_Saldo_Bancario.DTO
{
    public class HistoricoTransacaoDTO
    {
        public int Id { get; set; }
        public int? Tipo { get; set; }
        public decimal? Valor { get; set; }
        public string Data { get; set; }
    }
}
