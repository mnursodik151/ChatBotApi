public abstract class OpenAiObserver: IObserver<TelegramSendMessageRequestDto>
{
    protected readonly ITelegramMessageService _telegramMessageService;
    protected readonly long _chatId;
    protected readonly string _command;

    public OpenAiObserver(ITelegramMessageService telegramMessageService, long chat_id, ChatCommands command)
    {
        _telegramMessageService = telegramMessageService;
        _chatId = chat_id;
        _command = command.GetStringValue();
    }

    public long GetChatId() => _chatId;

    public string GetCommandName() => _command;

    public virtual void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public virtual void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public virtual void OnNext(TelegramSendMessageRequestDto value)
    {
        throw new NotImplementedException();
    }
}