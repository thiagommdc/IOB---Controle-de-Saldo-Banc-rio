using IOB___Controle_de_Saldo_Bancario.DTO;
using IOB___Controle_de_Saldo_Bancario.Model;

namespace IOB___Controle_de_Saldo_Bancario.Service
{
    public interface IServicoBancario
    {
        Task CreditarAsync(int contaId, decimal valor);
        Task DebitarAsync(int contaId, decimal valor);
        Task TransferirAsync(int contaOrigemId, int contaDestinoId, decimal valor);
        Task CriarContaBancariaAsync(string nome);
        Task<List<BankAccount>> ListarContasBancariasAsync();        
        Task<decimal> ConsultarSaldoAsync(int contaId);
        Task<List<BankLaunch>> ListarTransacoesAsync(int contaId);
    }
}
