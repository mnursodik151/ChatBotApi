using OpenAI_API.Completions;

public class OpenAiCompletionObserver : IObserver<CompletionResult>
{
    private readonly ITelegramMessageService _telegramMessageService;
    private readonly string _chatId;
    public OpenAiCompletionObserver(ITelegramMessageService telegramMessageService, string chat_id)
    {
        _telegramMessageService = telegramMessageService;
        _chatId = chat_id;
    }

    public async void OnCompleted()
    {
        await _telegramMessageService.SendMessageAsync(new TelegramSendMessageRequestDto
        {
            chat_id = _chatId,
            text = "Done"
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
            chat_id = _chatId,
            text = value.ToString()
        });
    }
}