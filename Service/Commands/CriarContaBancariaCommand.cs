using IOB___Controle_de_Saldo_Bancario.Enum;
using IOB___Controle_de_Saldo_Bancario.Model;
using IOB___Controle_de_Saldo_Bancario.Repository;

namespace IOB___Controle_de_Saldo_Bancario.Service.Commands
{
    public class CriarContaBancariaCommand : ICommand
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<BankAccount> _bankAccountRepository;
        private readonly IRepository<BankLaunch> _bankLaunchRepository;
        private readonly ILogger _logger;
        private readonly string _nome;
        private readonly decimal _saldoInicial = 0; 

        public CriarContaBancariaCommand(
            IUnitOfWork unitOfWork,
            IRepository<BankAccount> bankAccountRepository,
            IRepository<BankLaunch> bankLaunchRepository,
            ILogger logger,
            string nome,
            decimal saldoInicial = 0)
        {
            _unitOfWork = unitOfWork;
            _bankAccountRepository = bankAccountRepository;
            _bankLaunchRepository = bankLaunchRepository;
            _logger = logger;
            _nome = nome;
            _saldoInicial = saldoInicial;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var novaConta = new BankAccount
                {
                    Name = _nome,
                    Balance = _saldoInicial,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                await _bankAccountRepository.AddAsync(novaConta);

                if (_saldoInicial > 0)
                {
                    var lancamento = new BankLaunch
                    {
                        OriginBankAccountId = novaConta.Id,
                        Value = _saldoInicial,
                        OperationType = (int)OperationTypeEnum.Credit,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };
                    await _bankLaunchRepository.AddAsync(lancamento);
                }
                await _bankAccountRepository.AddAsync(novaConta);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Conta bancária criada com sucesso. Nome: {Nome}, Saldo inicial: {SaldoInicial}", _nome, _saldoInicial);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Erro ao criar a conta bancária. Nome: {Nome}", _nome);
                throw;
            }
        }
    }
}
