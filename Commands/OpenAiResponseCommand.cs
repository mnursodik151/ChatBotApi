public abstract class OpenAiResponseCommand : ICommand
{
    protected string _name;
    protected ITelegramMessageService _telegramMessageService; 
    protected readonly IOpenAiApiService _openAiApiService;
    protected ILogger _logger;
    private string name;
    private ILogger logger;
    private ITelegramMessageService telegramMessageService;

    public OpenAiResponseCommand(string name, ILogger logger, ITelegramMessageService telegramMessageService, IOpenAiApiService openAiApiService)
    {
        _name = name;
        _telegramMessageService = telegramMessageService;
        _openAiApiService = openAiApiService;
        _logger = logger;
    }

    protected OpenAiResponseCommand(string name, ILogger logger, ITelegramMessageService telegramMessageService)
    {
        this.name = name;
        this.logger = logger;
        this.telegramMessageService = telegramMessageService;
    }

    public virtual Task ExecuteAsync()
    {
        _logger.LogInformation("Regular Non Prompt Chat");
        throw new NotImplementedException();
    }

    protected TelegramSendMessageRequestDto GenerateSendMessageRequest(TelegramWebhookMessageDto input)
    {
        var result = new TelegramSendMessageRequestDto();
        int index = input.message.text.IndexOf(' ');
        if (index != -1)
        {
            result.chat_id = input.message.chat.id.ToString();
            result.text = input.message.text.Substring(index + 1);
        }

        return result;        
    }
}