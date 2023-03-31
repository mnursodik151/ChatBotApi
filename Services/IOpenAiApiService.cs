using OpenAI_API.Chat;
using OpenAI_API.Completions;

public interface IOpenAiApiService
{
    public OpenAiCompletionObservable GetCompletionObservable();
    public OpenAiChatObservable GetOpenAiConversationManager();

    public Task<CompletionResult> CreateCompletionAsync(CompletionRequest request);
    public Task StreamCompletionEnumerableAsync(CompletionRequest request);
    public IObserver<TelegramWebhookMessageDto> TryAddConversation(ITelegramMessageService telegramMessageService, string chat_id);
    public void AddChatMessage(TelegramWebhookMessageDto request);
}
