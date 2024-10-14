using Amazon.SQS.Model;

namespace IOB___Controle_de_Saldo_Bancario.Messaging
{
    public interface ISqsMessageProcessor
    {
        Task ProcessMessage(Message message);
        Task DeleteMessage(string queueUrl, string receiptHandle);
    }
}
