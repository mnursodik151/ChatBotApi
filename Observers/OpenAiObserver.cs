public abstract class OpenAiObserver
{
    protected readonly ITelegramMessageService _telegramMessageService;
    protected readonly string _chatId;
    protected readonly string _command;

    public OpenAiObserver(ITelegramMessageService telegramMessageService, string chat_id, string command)
    {
        _telegramMessageService = telegramMessageService;
        _chatId = chat_id;
        _command = command;
    }

    public string GetChatId() => _chatId;

    public string GetCommandName() => _command;
}