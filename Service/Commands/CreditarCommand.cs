using IOB___Controle_de_Saldo_Bancario.Enum;
using IOB___Controle_de_Saldo_Bancario.Model;
using IOB___Controle_de_Saldo_Bancario.Repository;

namespace IOB___Controle_de_Saldo_Bancario.Service.Commands
{
    public class CreditarCommand : ICommand
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<BankAccount> _bankAccountRepository;
        private readonly IRepository<BankLaunch> _bankLaunchRepository;
        private readonly ILogger _logger;
        private readonly int _contaId;
        private readonly decimal _valor;

        public CreditarCommand(IUnitOfWork unitOfWork,
                              IRepository<BankAccount> bankAccountRepository,
                              IRepository<BankLaunch> bankLaunchRepository,
                              ILogger logger,
                              int contaId,
                              decimal valor)
        {
            _unitOfWork = unitOfWork;
            _bankAccountRepository = bankAccountRepository;
            _bankLaunchRepository = bankLaunchRepository;
            _logger = logger;
            _contaId = contaId;
            _valor = valor;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var conta = await _bankAccountRepository.GetByIdAsync(_contaId);
                conta.Balance += _valor;
                await _bankAccountRepository.UpdateAsync(conta);

                var lancamento = new BankLaunch
                {
                    OriginBankAccountId = _contaId,
                    Value = _valor,
                    OperationType = (int)OperationTypeEnum.Credit,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };
                await _bankLaunchRepository.AddAsync(lancamento);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Crédito realizado com sucesso para a conta {ContaId}. Valor: {Valor}", _contaId, _valor);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Erro ao creditar a conta {ContaId}. Valor: {Valor}", _contaId, _valor);
                throw;
            }
        }
    }
}
