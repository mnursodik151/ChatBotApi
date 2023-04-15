using OpenAI_API.Chat;

public class OpenAiChatObservable : IObservable<TelegramSendMessageRequestDto>
{
    private readonly List<IObserver<TelegramSendMessageRequestDto>> _observers = new List<IObserver<TelegramSendMessageRequestDto>>();

    public IDisposable Subscribe(IObserver<TelegramSendMessageRequestDto> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }

        return new ObservableUnsubscriberUtil<TelegramSendMessageRequestDto>(_observers, observer);
    }

    public void Unsubscribe(long chat_id, string? command = null)
    {
        var observer = _observers.Where(obs => ((OpenAiObserver)obs).GetChatId() == chat_id);
        if(command != null)
            observer.Where(obs => ((OpenAiObserver)obs).GetCommandName() == command);
        if ( observer != null)
            _observers.Remove(observer.First());
    }

    public IObserver<TelegramSendMessageRequestDto>? GetObserver(long chat_id)
    {
        return _observers.FirstOrDefault(obs => ((OpenAiChatObserver)obs).GetChatId() == chat_id);
    }

    public Task AppendConversation(TelegramSendMessageRequestDto request)
    {
        var activeChat = _observers.First(observer =>
        ((OpenAiChatObserver)observer).GetChatId() == request.ChatId);
        activeChat.OnNext(request);
        return Task.CompletedTask;
    }
}