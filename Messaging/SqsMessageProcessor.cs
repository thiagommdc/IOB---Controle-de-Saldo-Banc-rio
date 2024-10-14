using Amazon.SQS;
using Amazon.SQS.Model;
using IOB___Controle_de_Saldo_Bancario.DTO;
using IOB___Controle_de_Saldo_Bancario.Enum;
using IOB___Controle_de_Saldo_Bancario.Service;
using IOB___Controle_de_Saldo_Bancario.Messaging;
using Newtonsoft.Json;

public class SqsMessageProcessor : ISqsMessageProcessor
{
    private readonly IAmazonSQS _sqsClient;
    private readonly ILogger<SqsMessageProcessor> _logger;
    private readonly IServicoBancario _servicoBancario;

    public SqsMessageProcessor(IAmazonSQS sqsClient, ILogger<SqsMessageProcessor> logger, IServicoBancario servicoBancario)
    {
        _sqsClient = sqsClient;
        _logger = logger;
        _servicoBancario = servicoBancario;
    }

    public async Task ProcessMessage(Message message)
    {
        var messageBody = JsonConvert.DeserializeObject<SqsMessageDTO>(message.Body);
        _logger.LogInformation($"Mensagem recebida: {message.Body}");

        switch (messageBody.TipoTransacao)
        {
            case OperationTypeEnum.Debit:
                await _servicoBancario.DebitarAsync(messageBody.ContaDestinoId, messageBody.Valor);
                break;
            case OperationTypeEnum.Credit:
                await _servicoBancario.CreditarAsync(messageBody.ContaDestinoId, messageBody.Valor);
                break;
            case OperationTypeEnum.Transfer:
                await _servicoBancario.TransferirAsync(messageBody.ContaOrigemId, messageBody.ContaDestinoId, messageBody.Valor);
                break;
            default:
                throw new InvalidOperationException("Operação inválida");
        }
    }

    public async Task DeleteMessage(string queueUrl, string receiptHandle)
    {
        await _sqsClient.DeleteMessageAsync(new DeleteMessageRequest
        {
            QueueUrl = queueUrl,
            ReceiptHandle = receiptHandle
        });
    }
}