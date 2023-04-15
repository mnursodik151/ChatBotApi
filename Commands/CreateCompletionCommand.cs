using OpenAI_API.Completions;

public class CreateCompletionCommand : OpenAiResponseCommand
{
    private CompletionRequest _request;
    public CreateCompletionCommand(ChatCommands name, string message, ILogger logger, ITelegramMessageService telegramMessageService, IOpenAiApiService openAiApiService) 
    : base(name, logger, telegramMessageService, openAiApiService)
    {
        _request = new CompletionRequest(message, model: OpenAI_API.Models.Model.DavinciText, temperature: 0.6);
    }

    public override async Task ExecuteAsync()
    {
        await _openAiApiService.CreateCompletionAsync(_request);
    }
}
