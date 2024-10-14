using IOB___Controle_de_Saldo_Bancario.Model;
using IOB___Controle_de_Saldo_Bancario.Repository;

namespace IOB___Controle_de_Saldo_Bancario.Service.Commands
{
    public class ConsultarSaldoCommand : ICommand<decimal>
    {
        private readonly IRepository<BankAccount> _bankAccountRepository;
        private readonly ILogger _logger;
        private readonly int _contaId;

        public ConsultarSaldoCommand(
            IRepository<BankAccount> bankAccountRepository,
            ILogger logger,
            int contaId)
        {
            _bankAccountRepository = bankAccountRepository;
            _logger = logger;
            _contaId = contaId;
        }

        public async Task<decimal> ExecuteAsync()
        {
            try
            {
                var conta = await _bankAccountRepository.GetByIdAsync(_contaId);

                if (conta == null)
                {
                    _logger.LogWarning("Erro ao consultar o saldo. Conta: {ContaId}", _contaId);
                    throw new InvalidOperationException("Conta não encontrada.");
                }

                _logger.LogInformation("Saldo consultado com sucesso. Conta: {ContaId}, Saldo: {Saldo}", _contaId, conta.Balance);
                return conta.Balance;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar o saldo. Conta: {ContaId}", _contaId);
                throw;
            }
        }
    }
}
