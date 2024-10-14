using Amazon.SQS;
using Amazon.SQS.Model;
using IOB___Controle_de_Saldo_Bancario.Messaging;

public class SqsMessageRetryHandler : ISqsMessageRetryHandler
{
    private readonly IAmazonSQS _sqsClient;
    private readonly int _retryCount;
    private readonly string _deadLetterQueueUrl;
    private readonly ISqsMessageProcessor _messageProcessor;
    private readonly ILogger<SqsMessageRetryHandler> _logger;

    public SqsMessageRetryHandler(IAmazonSQS sqsClient, 
        IConfiguration configuration, 
        ISqsMessageProcessor messageProcessor, 
        ILogger<SqsMessageRetryHandler> logger)
    {
        _sqsClient = sqsClient;
        _retryCount = configuration.GetValue<int>("SqsSettings:RetryCount");
        _deadLetterQueueUrl = configuration.GetValue<string>("SqsSettings:eadLetterQueueUrl");
        _messageProcessor = messageProcessor;
        _logger = logger;
    }

    public async Task HandleRetry(Message message)
    {
        int retryCount = 0;
        while (retryCount < _retryCount)
        {
            try
            {
                await _messageProcessor.ProcessMessage(message);
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao processar a mensagem: {ex.Message}");
                retryCount++;
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, retryCount)));
            }
        }

        if (retryCount == _retryCount)
        {
            await _sqsClient.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = _deadLetterQueueUrl,
                MessageBody = message.Body
            });
        }
    }
}