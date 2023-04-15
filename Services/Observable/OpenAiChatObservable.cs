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
        if (command != null)
            observer.Where(obs => ((OpenAiObserver)obs).GetCommandName() == command);
        if (observer != null)
            _observers.Remove(observer.First());
    }

    public IObserver<TelegramSendMessageRequestDto>? GetObserver(long chat_id, ChatCommands command)
    {
        return _observers
            .OfType<OpenAiObserver>()
            .FirstOrDefault(obs => obs.GetChatId() == chat_id && obs.GetCommandName() == command.GetStringValue());
    }

    public Task AppendConversation(TelegramSendMessageRequestDto request, ChatCommands command)
    {
        OpenAiObserver? activeChat = null;

        switch (command)
        {
            case ChatCommands.Chat:
                activeChat = _observers.OfType<OpenAiChatObserver>()
                    .FirstOrDefault(obs => obs.GetChatId() == request.ChatId && obs.GetCommandName() == command.GetStringValue());
                break;
            case ChatCommands.Voice:
                activeChat = _observers.OfType<OpenAiVoiceChatObserver>()
                    .FirstOrDefault(obs => obs.GetChatId() == request.ChatId && obs.GetCommandName() == command.GetStringValue());
                break;
        }

        if (activeChat != null)
        {
            activeChat.OnNext(request);
        }

        return Task.CompletedTask;
    }

}