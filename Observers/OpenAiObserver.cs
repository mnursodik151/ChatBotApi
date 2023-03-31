public abstract class OpenAiObserver
{
    protected readonly ITelegramMessageService _telegramMessageService;
    protected readonly string _chatId;

    public OpenAiObserver(ITelegramMessageService telegramMessageService, string chat_id)
    {
        _telegramMessageService = telegramMessageService;
        _chatId = chat_id;
    }
}