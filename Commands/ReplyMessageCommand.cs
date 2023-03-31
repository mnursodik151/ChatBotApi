public class ReplyMessageCommand : OpenAiResponseCommand, ICommand
{
    private readonly TelegramWebhookMessageDto _requestDto;
    public ReplyMessageCommand(string name, TelegramWebhookMessageDto message, ILogger logger, ITelegramMessageService telegramMessageService, IOpenAiApiService openAiApiService) 
    : base(name, logger, telegramMessageService, openAiApiService)
    {
        _requestDto = message;
    }

    public override Task ExecuteAsync()
    {
        var requestDto = GenerateSendMessageRequest(_requestDto);
        _openAiApiService.AddChatMessage(requestDto);
        return Task.CompletedTask;
    }
}
