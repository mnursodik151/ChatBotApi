public class ResetTopicCommand : OpenAiResponseCommand, ICommand
{
    private string _chat_id;

    public ResetTopicCommand(string name, string chat_id, ILogger logger, ITelegramMessageService telegramMessageService, IOpenAiApiService openAiApiService) 
    : base(name, logger, telegramMessageService, openAiApiService)
    {
        _chat_id = chat_id;
    }

    public override Task ExecuteAsync()
    {
        _openAiApiService.GetOpenAiConversationManager().Unsubscribe(_chat_id);
        return Task.CompletedTask;
    }
}
