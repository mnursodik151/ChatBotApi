public interface ICommand
{
    public ChatCommands GetCommandName();
    public Task ExecuteAsync();
}