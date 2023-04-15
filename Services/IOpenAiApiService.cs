using OpenAI_API.Chat;
using OpenAI_API.Completions;

public interface IOpenAiApiService
{
    public OpenAiCompletionObservable GetCompletionObservable();
    public OpenAiChatObservable GetOpenAiConversationManager();

    public Task<CompletionResult> CreateCompletionAsync(CompletionRequest request);
    public Task StreamCompletionEnumerableAsync(CompletionRequest request);
    public IObserver<TelegramSendMessageRequestDto> TryAddConversation(ITelegramMessageService telegramMessageService, long chat_id, ChatCommands command);
    public Task AddChatMessage(TelegramSendMessageRequestDto request, ChatCommands command);
}
