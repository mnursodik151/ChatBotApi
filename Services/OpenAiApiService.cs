using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Completions;

public class OpenAiApiService : IOpenAiApiService
{
    private readonly string? _openAiApiToken;
    private readonly ILogger<IOpenAiApiService> _logger;
    private readonly OpenAiCompletionObservable _openAiCompletionObservable;
    private readonly OpenAiChatObservable _openAiConversationManager;
    private readonly OpenAIAPI _openAiApi;

    public OpenAiApiService(IConfiguration configuration, ILogger<IOpenAiApiService> logger,
    IObservable<CompletionResult> openAiCompletionObservable, 
    IObservable<TelegramWebhookMessageDto> openAiChatObservable)
    {
        _logger = logger;
        _openAiApiToken = configuration["OpenAiAPIKey"];
        _openAiApi = new OpenAIAPI(_openAiApiToken);

        _openAiCompletionObservable = (OpenAiCompletionObservable)openAiCompletionObservable;
        _openAiConversationManager = (OpenAiChatObservable)openAiChatObservable;
    }
    
    public OpenAiCompletionObservable GetCompletionObservable() => _openAiCompletionObservable;
    public OpenAiChatObservable GetOpenAiConversationManager() => _openAiConversationManager;

    public async Task<CompletionResult> CreateCompletionAsync(CompletionRequest request)
    {
        return await _openAiApi.Completions.CreateCompletionAsync(request);
    }    

    public async Task StreamCompletionEnumerableAsync(CompletionRequest request)
    {
        var completionStream = await _openAiApi.Completions.CreateCompletionAsync(request);
        _openAiCompletionObservable.StartListening(completionStream);                     
    }

    public IObserver<TelegramWebhookMessageDto> TryAddConversation(ITelegramMessageService telegramMessageService, string chat_id)
    {
        var conversation = _openAiConversationManager.GetObserver(chat_id);
        if(conversation == null)
        {            
            conversation = new OpenAiChatObserver(_openAiApi, telegramMessageService, chat_id, _logger);
            _openAiConversationManager.Subscribe(conversation);
            _logger.LogInformation($"Conversation with {chat_id} created");
        } 
        
        return conversation;
    }

    public void AddChatMessage(TelegramWebhookMessageDto request)
    {
        _openAiConversationManager.AppendConversation(request);
    }
}