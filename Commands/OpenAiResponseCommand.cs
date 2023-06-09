public abstract class OpenAiResponseCommand : ICommand
{
    protected ChatCommands _name;
    protected ITelegramMessageService _telegramMessageService; 
    protected readonly IOpenAiApiService _openAiApiService;
    protected ILogger _logger;
    private ILogger logger;
    private ITelegramMessageService telegramMessageService;

    public OpenAiResponseCommand(ChatCommands name,
                                 ILogger logger,
                                 ITelegramMessageService telegramMessageService,
                                 IOpenAiApiService openAiApiService)
    {
        _name = name;
        _telegramMessageService = telegramMessageService;
        _openAiApiService = openAiApiService;
        _logger = logger;
    }

    public ChatCommands GetCommandName() => _name;

    public virtual Task ExecuteAsync()
    {
        return Task.CompletedTask;
    }

    protected TelegramSendMessageRequestDto GenerateSendMessageRequest(TelegramWebhookMessageDto input)
    {
        var result = new TelegramSendMessageRequestDto();
        int index = input.message.text.IndexOf(' ');
        if (index != -1)
        {
            result.ChatId = input.message.chat.id;
            result.Text = input.message.text.Substring(index + 1);
        }

        return result;        
    }
}