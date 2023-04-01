using OpenAI_API.Chat;
using OpenAI_API.Completions;

public interface IOpenAiApiService
{
    public OpenAiCompletionObservable GetCompletionObservable();
    public OpenAiChatObservable GetOpenAiConversationManager();

    public Task<CompletionResult> CreateCompletionAsync(CompletionRequest request);
    public Task StreamCompletionEnumerableAsync(CompletionRequest request);
    public IObserver<TelegramSendMessageRequestDto> TryAddConversation(ITelegramMessageService telegramMessageService, string chat_id, string command);
    public Task AddChatMessage(TelegramSendMessageRequestDto request);
}
