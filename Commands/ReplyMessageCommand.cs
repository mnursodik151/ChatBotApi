public class ReplyMessageCommand : OpenAiResponseCommand
{
    private readonly TelegramWebhookMessageDto _requestDto;
    public ReplyMessageCommand(ChatCommands name, TelegramWebhookMessageDto message, ILogger logger, ITelegramMessageService telegramMessageService, IOpenAiApiService openAiApiService) 
    : base(name, logger, telegramMessageService, openAiApiService)
    {
        _requestDto = message;
    }

    public override async Task ExecuteAsync()
    {
        var requestDto = GenerateSendMessageRequest(_requestDto);
        await _openAiApiService.AddChatMessage(requestDto, _name);
    }
}
