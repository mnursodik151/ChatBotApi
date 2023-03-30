using OpenAI_API;
using OpenAI_API.Completions;

public class OpenAiApiService : IOpenAiApiService
{
    private readonly string? _openAiApiToken;
    private readonly OpenAiCompletionObservable _openAiCompletionObservable;
    private readonly OpenAIAPI _openAiApi;

    public OpenAiApiService(IConfiguration configuration, 
    IObservable<CompletionResult> openAiCompletionObservable, 
    IObserver<CompletionResult> openAiCompletionObserver)
    {
        _openAiApiToken = configuration["OpenAiAPIKey"];
        _openAiApi = new OpenAIAPI(_openAiApiToken);

        _openAiCompletionObservable = (OpenAiCompletionObservable)openAiCompletionObservable;
    }
    public OpenAiCompletionObservable GetCompletionObservable() => _openAiCompletionObservable;

    public async Task<CompletionResult> CreateCompletionAsync(CompletionRequest request)
    {
        return await _openAiApi.Completions.CreateCompletionAsync(request);
    }    

    public async Task StreamCompletionEnumerableAsync(CompletionRequest request)
    {
        var completionStream = _openAiApi.Completions.StreamCompletionEnumerableAsync(request);
        await _openAiCompletionObservable.StartListeningAsync(completionStream);                     
    }
}