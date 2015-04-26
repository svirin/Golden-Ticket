namespace GoldenTicket.Command.Interfaces
{
    public interface ICommandFactory<in TEntity>
        where TEntity : class, new()
    {
        ICommand<TEntity> CreateCommand();
    }
}
