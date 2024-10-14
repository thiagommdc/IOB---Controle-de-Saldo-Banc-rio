using Amazon.SQS.Model;

namespace IOB___Controle_de_Saldo_Bancario.Messaging
{
    public interface ISqsMessageRetryHandler
    {
        Task HandleRetry(Message message);
    }
}
