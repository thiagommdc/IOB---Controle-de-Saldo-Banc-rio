namespace IOB___Controle_de_Saldo_Bancario.Messaging
{
    public interface ISqsMessagePoller
    {
        Task PollMessages();
    }
}
