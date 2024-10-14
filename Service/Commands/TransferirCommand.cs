using IOB___Controle_de_Saldo_Bancario.Model;
using IOB___Controle_de_Saldo_Bancario.Repository;
using Microsoft.OpenApi.Models;

namespace IOB___Controle_de_Saldo_Bancario.Service.Commands
{
    public class TransferirCommand : ICommand
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<BankAccount> _bankAccountRepository;
        private readonly IRepository<BankLaunch> _bankLaunchRepository;
        private readonly ILogger _logger;
        private readonly int _contaOrigemId;
        private readonly int _contaDestinoId;
        private readonly decimal _valor;

        public TransferirCommand(
            IUnitOfWork unitOfWork,
            IRepository<BankAccount> bankAccountRepository,
            IRepository<BankLaunch> bankLaunchRepository,
            ILogger logger,
            int contaOrigemId,
            int contaDestinoId,
            decimal valor)
        {
            _unitOfWork = unitOfWork;
            _bankAccountRepository = bankAccountRepository;
            _bankLaunchRepository = bankLaunchRepository;
            _logger = logger;
            _contaOrigemId = contaOrigemId;
            _contaDestinoId = contaDestinoId;
            _valor = valor;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var contaOrigem = await _bankAccountRepository.GetByIdAsync(_contaOrigemId);
                var contaDestino = await _bankAccountRepository.GetByIdAsync(_contaDestinoId);

                if (contaOrigem.Balance < _valor)
                {
                    _logger.LogWarning("Saldo insuficiente para a transferência. Conta origem: {ContaOrigemId}, Conta destino: {ContaDestinoId}, Valor: {Valor}", _contaOrigemId, _contaDestinoId, _valor);
                    throw new InvalidOperationException("Saldo insuficiente para a transferência.");
                }

                contaOrigem.Balance -= _valor;
                await _bankAccountRepository.UpdateAsync(contaOrigem);

                contaDestino.Balance += _valor;
                await _bankAccountRepository.UpdateAsync(contaDestino);

                var lancamento = new BankLaunch
                {
                    OriginBankAccountId = _contaOrigemId,
                    DestinationBankAccountId = _contaDestinoId,
                    Value = _valor,
                    OperationType = (int)OperationTypeEnum.Transfer,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };
                await _bankLaunchRepository.AddAsync(lancamento);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Transferência realizada com sucesso. Conta origem: {ContaOrigemId}, Conta destino: {ContaDestinoId}, Valor: {Valor}", _contaOrigemId, _contaDestinoId, _valor);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Erro ao realizar a transferência. Conta origem: {ContaOrigemId}, Conta destino: {ContaDestinoId}, Valor: {Valor}", _contaOrigemId, _contaDestinoId, _valor);
                throw;
            }
        }
    }
}
