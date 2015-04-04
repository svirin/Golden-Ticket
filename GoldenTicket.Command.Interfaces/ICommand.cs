namespace GoldenTicket.Command.Interfaces
{
    public interface ICommand<in TEntity>
        where TEntity : class, new()
    {
        void ExecuteCommand(TEntity item);
    }
}
