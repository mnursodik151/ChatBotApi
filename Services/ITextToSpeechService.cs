public interface ITextToSpeechService
{
    Task<Stream> GenerateAudioStreamAsync(string text);
}