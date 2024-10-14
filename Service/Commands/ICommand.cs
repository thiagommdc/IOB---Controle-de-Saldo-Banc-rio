namespace IOB___Controle_de_Saldo_Bancario.Service.Commands
{
    public interface ICommand<T>
    {
        Task<T> ExecuteAsync();
    }

    public interface ICommand
    {
        Task ExecuteAsync();
    }
}
