using Google.Apis.Auth.OAuth2;
using Google.Cloud.TextToSpeech.V1;
using System.IO;
using System.Threading.Tasks;

public class GCPTextToSpeechService : ITextToSpeechService
{
    private readonly TextToSpeechClient _textToSpeechClient;
    private readonly TextLanguageDetectionUtil _textLanguageDetectionUtil;

    public GCPTextToSpeechService(IConfiguration configuration, TextLanguageDetectionUtil textLanguageDetectionUtil)
    {
        var credentialsFilePath = configuration["GCPCredentialPath"];

        _textLanguageDetectionUtil = textLanguageDetectionUtil;

        var builder = new TextToSpeechClientBuilder();
        builder.CredentialsPath = credentialsFilePath;
        _textToSpeechClient = builder.Build();
    }

    public async Task<byte[]> GenerateAudioByteAsync(string text)
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
            SsmlGender = SsmlVoiceGender.Male
        };

        var detectedLanguage = _textLanguageDetectionUtil.DetectLanguageFromText(text);
        if (detectedLanguage != null && LanguageCodeMappingUtil.Iso639_3ToGcp.TryGetValue(detectedLanguage.Iso639_3, out string? gcpLanguageCode))
        {
            voiceSelection.LanguageCode = gcpLanguageCode;
            voiceSelection.SsmlGender = SsmlVoiceGender.Male;
        }

        // Select the type of audio file you want returned.
        var audioConfig = new AudioConfig
        {
            AudioEncoding = AudioEncoding.OggOpus
        };

        // Perform the text-to-speech request.
        var response = await _textToSpeechClient.SynthesizeSpeechAsync(input, voiceSelection, audioConfig);

        return response.AudioContent.ToByteArray();

        // // Get the audio content as a stream.
        // var audioStream = new MemoryStream(response.AudioContent.ToByteArray());

        // // Set the stream position to the beginning so that it can be read from the start.
        // audioStream.Position = 0;

        // return audioStream;
    }
}
