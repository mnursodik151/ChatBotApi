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
    IObservable<TelegramSendMessageRequestDto> openAiChatObservable)
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

    public IObserver<TelegramSendMessageRequestDto> TryAddConversation(ITelegramMessageService telegramMessageService, long chat_id, ChatCommands command)
    {
        var conversation = _openAiConversationManager.GetObserver(chat_id, command);
        if (conversation == null)
        {
            switch (command)
            {
                case ChatCommands.Dm:
                case ChatCommands.Chat:
                    conversation = new OpenAiChatObserver(_openAiApi, telegramMessageService, _logger, chat_id, command);
                    break;
                case ChatCommands.Voice:
                    conversation = new OpenAiVoiceChatObserver(_openAiApi, telegramMessageService, _logger, chat_id, command);
                    break;
            }

            _openAiConversationManager.Subscribe(conversation);
            _logger.LogInformation($"Conversation with {chat_id} created");
        }

        return conversation;
    }

    public Task AddChatMessage(TelegramSendMessageRequestDto request, ChatCommands command)
    {
        _openAiConversationManager.AppendConversation(request, command);
        return Task.CompletedTask;
    }
}