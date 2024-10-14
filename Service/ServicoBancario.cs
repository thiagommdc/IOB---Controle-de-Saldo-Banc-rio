using IOB___Controle_de_Saldo_Bancario.DTO;
using IOB___Controle_de_Saldo_Bancario.Model;
using IOB___Controle_de_Saldo_Bancario.Repository;
using IOB___Controle_de_Saldo_Bancario.Service.Commands;

namespace IOB___Controle_de_Saldo_Bancario.Service
{
    public class ServicoBancario : IServicoBancario
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<BankAccount> _bankAccountRepository;
        private readonly IRepository<BankLaunch> _bankLaunchRepository;
        private readonly ILogger<ServicoBancario> _logger;

        public ServicoBancario(IUnitOfWork unitOfWork,
                              IRepository<BankAccount> bankAccountRepository,
                              IRepository<BankLaunch> bankLaunchRepository,
                              ILogger<ServicoBancario> logger)
        {
            _unitOfWork = unitOfWork;
            _bankAccountRepository = bankAccountRepository;
            _bankLaunchRepository = bankLaunchRepository;
            _logger = logger;
        }

        public async Task CreditarAsync(int contaId, decimal valor)
        {
            await new CreditarCommand(_unitOfWork, _bankAccountRepository, _bankLaunchRepository, _logger, contaId, valor).ExecuteAsync();
        }

        public async Task DebitarAsync(int contaId, decimal valor)
        {
            await new DebitarCommand(_unitOfWork, _bankAccountRepository, _bankLaunchRepository, _logger, contaId, valor).ExecuteAsync();
        }

        public async Task TransferirAsync(int contaOrigemId, int contaDestinoId, decimal valor)
        {
            await new TransferirCommand(_unitOfWork, _bankAccountRepository, _bankLaunchRepository, _logger, contaOrigemId, contaDestinoId, valor).ExecuteAsync();
        }

        public async Task CriarContaBancariaAsync(string nome)
        {
            await new CriarContaBancariaCommand(_unitOfWork, _bankAccountRepository, _bankLaunchRepository, _logger, nome).ExecuteAsync();
        }

        public async Task<decimal> ConsultarSaldoAsync(int contaId)
        {
            return await new ConsultarSaldoCommand(_bankAccountRepository, _logger, contaId).ExecuteAsync();
        }

        public async Task<List<BankLaunch>> ListarTransacoesAsync(int contaId)
        {
            return await new ListarTransacoesCommand(_bankLaunchRepository, _logger, contaId).ExecuteAsync();
        }

        public async Task<List<BankAccount>> ListarContasBancariasAsync()
        {
            return await new ListarContasBancariasCommand(_bankAccountRepository, _logger).ExecuteAsync();
        }
    }
}
