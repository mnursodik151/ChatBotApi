using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class TelegramMessageService : ITelegramMessageService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerSettings _jsonSerializerSettings;
    private readonly string _telegramBotToken;

    public TelegramMessageService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _telegramBotToken = configuration["TelegramBotToken"];
        _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };
    }

    public async Task<TelegramSendMessageResponseDto> SendMessageAsync(TelegramSendMessageRequestDto request)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"bot{_telegramBotToken}/sendMessage");
        httpRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"chat_id", request.chat_id},
            {"text", request.text}
        });

        var httpResponse = await _httpClient.SendAsync(httpRequest);
        httpResponse.EnsureSuccessStatusCode();

        var responseJson = await httpResponse.Content.ReadAsStringAsync();
        var sendMessageResponse = JsonConvert.DeserializeObject<TelegramSendMessageResponseDto>(responseJson, _jsonSerializerSettings);

        return sendMessageResponse;
    }
}