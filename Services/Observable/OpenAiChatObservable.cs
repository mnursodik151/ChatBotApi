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

    public void Unsubscribe(string chat_id)
    {
        var observer = _observers.FirstOrDefault(obs => ((OpenAiChatObserver)obs).GetChatId() == chat_id);
        if ( observer != null)
            _observers.Remove(observer);
    }

    public IObserver<TelegramSendMessageRequestDto>? GetObserver(string chat_id)
    {
        return _observers.FirstOrDefault(obs => ((OpenAiChatObserver)obs).GetChatId() == chat_id);
    }

    public Task AppendConversation(TelegramSendMessageRequestDto request)
    {
        var activeChat = _observers.First(observer =>
        ((OpenAiChatObserver)observer).GetChatId() == request.chat_id);
        activeChat.OnNext(request);
        return Task.CompletedTask;
    }
}