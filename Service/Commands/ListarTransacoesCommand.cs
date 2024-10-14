using IOB___Controle_de_Saldo_Bancario.Model;
using IOB___Controle_de_Saldo_Bancario.Repository;

namespace IOB___Controle_de_Saldo_Bancario.Service.Commands
{
    public class ListarTransacoesCommand : ICommand<List<BankLaunch>>
    {

        private readonly IRepository<BankLaunch> _bankLaunchRepository;
        private readonly ILogger _logger;
        private readonly int _contaId;

        public ListarTransacoesCommand(
            IRepository<BankLaunch> bankLaunchRepository,
            ILogger logger,
            int contaId)
        {
            _bankLaunchRepository = bankLaunchRepository;
            _logger = logger;
            _contaId = contaId;
        }

        public async Task<List<BankLaunch>> ExecuteAsync()
        {
            try
            {
                return await _bankLaunchRepository.GetAllAsync(_contaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar transações. Conta: {contaId}", _contaId);
                throw;
            }
        }
    }
}
