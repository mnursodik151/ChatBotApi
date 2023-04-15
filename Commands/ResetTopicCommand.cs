public class ResetTopicCommand : OpenAiResponseCommand
{
    private long _chat_id;
    private string? _command;

    public ResetTopicCommand(ChatCommands name, long chat_id, ILogger logger, ITelegramMessageService telegramMessageService, IOpenAiApiService openAiApiService, string? command = null) 
    : base(name, logger, telegramMessageService, openAiApiService)
    {
        _chat_id = chat_id;
        _command = command;
    }

    public override Task ExecuteAsync()
    {
        _openAiApiService.GetOpenAiConversationManager().Unsubscribe(_chat_id, _command);
        return Task.CompletedTask;
    }
}
