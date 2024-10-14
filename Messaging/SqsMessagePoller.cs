using Amazon.SQS.Model;
using Amazon.SQS;
using IOB___Controle_de_Saldo_Bancario.Messaging;

public class SqsMessagePoller : ISqsMessagePoller
{
    private readonly IAmazonSQS _sqsClient;
    private readonly string _queueUrl;
    private readonly int _maxMessagesPerRequest;
    private readonly int _waitTimeInSeconds;
    private readonly ISqsMessageProcessor _messageProcessor;
    private readonly ISqsMessageRetryHandler _retryHandler;
    private readonly ILogger<SqsMessagePoller> _logger;

    public SqsMessagePoller(IAmazonSQS sqsClient, IConfiguration configuration,
                            ISqsMessageProcessor messageProcessor, ISqsMessageRetryHandler retryHandler,
                            ILogger<SqsMessagePoller> logger)
    {
        _sqsClient = sqsClient;
        _queueUrl = configuration.GetValue<string>("SqsSettings:QueueUrl");
        _maxMessagesPerRequest = configuration.GetValue<int>("SqsSettings:MaxMessagesPerRequest");
        _waitTimeInSeconds = configuration.GetValue<int>("SqsSettings:WaitTimeInSeconds");
        _messageProcessor = messageProcessor;
        _retryHandler = retryHandler;
        _logger = logger;
    }

    public async Task PollMessages()
    {
        var request = new ReceiveMessageRequest
        {
            QueueUrl = _queueUrl,
            MaxNumberOfMessages = _maxMessagesPerRequest,
            WaitTimeSeconds = _waitTimeInSeconds
        };

        var response = await _sqsClient.ReceiveMessageAsync(request);

        foreach (var message in response.Messages)
        {
            try
            {
                await _messageProcessor.ProcessMessage(message);
                await _messageProcessor.DeleteMessage(_queueUrl, message.ReceiptHandle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar a mensagem: {MessageId}", message.MessageId);
                await _retryHandler.HandleRetry(message);
            }
        }
    }
}