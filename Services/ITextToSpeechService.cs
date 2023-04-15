public interface ITextToSpeechService
{
    Task<byte[]> GenerateAudioByteAsync(string text);
}