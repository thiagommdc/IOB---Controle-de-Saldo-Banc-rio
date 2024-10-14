using IOB___Controle_de_Saldo_Bancario.Model;
using IOB___Controle_de_Saldo_Bancario.Repository;

namespace IOB___Controle_de_Saldo_Bancario.Service.Commands
{
    public class ListarContasBancariasCommand
    {
        private readonly IRepository<BankAccount> _bankAccountRepository;
        private readonly ILogger _logger;

        public ListarContasBancariasCommand(
            IRepository<BankAccount> bankAccountRepository,
            ILogger logger)
        {
            _bankAccountRepository = bankAccountRepository;
            _logger = logger;
        }

        public async Task<List<BankAccount>> ExecuteAsync()
        {
            try
            {
                return await _bankAccountRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar contas bancárias");
                throw;
            }
        }
    }
}
