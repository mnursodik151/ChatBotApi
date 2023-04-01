public interface ICommand
{
    public string GetCommandName();
    public Task ExecuteAsync();
}