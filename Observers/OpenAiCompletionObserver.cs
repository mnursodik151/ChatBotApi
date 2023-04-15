using OpenAI_API.Completions;

public class OpenAiCompletionObserver : OpenAiObserver, IObserver<CompletionResult>
{
    public OpenAiCompletionObserver(ITelegramMessageService telegramMessageService, long chat_id, string command) 
    : base(telegramMessageService, chat_id, command)
    {
    }

    public async void OnCompleted()
    {
        await _telegramMessageService.SendMessageAsync(new TelegramSendMessageRequestDto
        {
            ChatId = _chatId,
            Text = "Done"
        });
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public async void OnNext(CompletionResult value)
    {
        await _telegramMessageService.SendMessageAsync(new TelegramSendMessageRequestDto
        {
            ChatId = _chatId,
            Text = value.ToString()
        });
    }
}