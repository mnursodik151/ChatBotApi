using OpenAI_API.Completions;

public interface IOpenAiApiService
{
    public OpenAiCompletionObservable GetCompletionObservable();
    public Task<CompletionResult> CreateCompletionAsync(CompletionRequest request);
    public Task StreamCompletionEnumerableAsync(CompletionRequest request);
}
