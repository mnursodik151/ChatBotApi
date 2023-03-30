using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class TelegramMessageService : ITelegramMessageService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerSettings _jsonSerializerSettings;
    private readonly string? _telegramBotToken;

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
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var httpResponse = await _httpClient.PostAsync($"bot{_telegramBotToken}/sendMessage", content);
        httpResponse.EnsureSuccessStatusCode();

        var responseJson = await httpResponse.Content.ReadAsStringAsync();
        var sendMessageResponse = JsonConvert.DeserializeObject<TelegramSendMessageResponseDto>(responseJson, _jsonSerializerSettings);

        if(sendMessageResponse == null)
            throw new HttpRequestException(HttpStatusCode.NotFound.ToString());

        return sendMessageResponse;
    }
}