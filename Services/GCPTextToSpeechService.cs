using Google.Apis.Auth.OAuth2;
using Google.Cloud.TextToSpeech.V1;
using System.IO;
using System.Threading.Tasks;

public class GCPTextToSpeechService : ITextToSpeechService
{
    private readonly TextToSpeechClient _textToSpeechClient;

    public GCPTextToSpeechService(IConfiguration configuration)
    {
        var credentialsFilePath = configuration["GCPCredentialPath"];

        var builder = new TextToSpeechClientBuilder();
        builder.CredentialsPath = credentialsFilePath;

        _textToSpeechClient = builder.Build();
    }

    public async Task<Stream> GenerateAudioStreamAsync(string text)
    {
        // Set the text input to be synthesized.
        var input = new SynthesisInput
        {
            Text = text
        };

        // Build the voice request.
        var voiceSelection = new VoiceSelectionParams
        {
            LanguageCode = "en-US",
            SsmlGender = SsmlVoiceGender.Neutral
        };

        // Select the type of audio file you want returned.
        var audioConfig = new AudioConfig
        {
            AudioEncoding = AudioEncoding.Mp3
        };

        // Perform the text-to-speech request.
        var response = await _textToSpeechClient.SynthesizeSpeechAsync(input, voiceSelection, audioConfig);

        // Get the audio content as a stream.
        var audioStream = new MemoryStream(response.AudioContent.ToByteArray());

        // Set the stream position to the beginning so that it can be read from the start.
        audioStream.Position = 0;

        return audioStream;
    }
}
