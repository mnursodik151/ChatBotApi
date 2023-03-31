using OpenAI_API.Chat;

public class OpenAiChatObservable : IObservable<TelegramWebhookMessageDto>
{
    private readonly List<IObserver<TelegramWebhookMessageDto>> _observers = new List<IObserver<TelegramWebhookMessageDto>>();

    public IDisposable Subscribe(IObserver<TelegramWebhookMessageDto> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }

        return new ObservableUnsubscriberUtil<TelegramWebhookMessageDto>(_observers, observer);
    }

    public void Unsubscribe(IObserver<TelegramWebhookMessageDto> observer)
    {
        if (_observers.Contains(observer))
            _observers.Remove(observer);
    }

    public IObserver<TelegramWebhookMessageDto>? GetObserver(string chat_id)
    {
        return _observers.FirstOrDefault(obs => ((OpenAiChatObserver)obs).GetChatId() == chat_id);
    }

    public void AppendConversation(TelegramWebhookMessageDto request)
    {
        var activeChat = _observers.First(observer =>
        ((OpenAiChatObserver)observer).GetChatId() == request.message.chat.id);
        activeChat.OnNext(request);
    }
}