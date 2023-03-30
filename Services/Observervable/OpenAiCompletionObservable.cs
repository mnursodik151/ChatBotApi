using OpenAI_API.Completions;

public class OpenAiCompletionObservable : IObservable<CompletionResult>
{
    private readonly List<IObserver<CompletionResult>> _observers = new List<IObserver<CompletionResult>>();

    public IDisposable Subscribe(IObserver<CompletionResult> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }

        return new ObservableUnsubscriberUtil<CompletionResult>(_observers, observer);
    }

    public async Task StartListeningAsync(IAsyncEnumerable<CompletionResult> results)
    {
        await foreach (var token in results)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(token);
            }
        }

        foreach (var observer in _observers.ToArray())
        {
            observer.OnCompleted();
        }
    }
}