using IOB___Controle_de_Saldo_Bancario.Messaging;

public class SqsConsumerService : IHostedService
{
    private readonly ISqsMessagePoller _messagePoller;
    private readonly ILogger<SqsConsumerService> _logger;
    private Timer _timer;
    private readonly int _waitTimeInSeconds;

    public SqsConsumerService(ISqsMessagePoller messagePoller, ILogger<SqsConsumerService> logger, IConfiguration configuration)
    {
        _messagePoller = messagePoller;
        _logger = logger;
        _waitTimeInSeconds = configuration.GetValue<int>("SqsSettings:WaitTimeInSeconds");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(async state => await ProcessMessages(), null, TimeSpan.Zero, TimeSpan.FromSeconds(_waitTimeInSeconds));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private async Task ProcessMessages()
    {
        try
        {
            await _messagePoller.PollMessages();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar mensagens da fila SQS");
        }
    }
}