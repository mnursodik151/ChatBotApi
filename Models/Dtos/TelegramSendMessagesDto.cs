using Newtonsoft.Json;

public class TelegramSendMessageRequestDto
{
    [JsonProperty]
    public long ChatId { get; set; }
    [JsonProperty]
    public string? Text { get; set; }
}

public class TelegramSendVoiceRequestDto
{
    [JsonProperty]
    public long ChatId { get; set; }
    [JsonProperty]
    public string? VoiceName { get; set; }
    [JsonProperty]
    public byte[]? VoiceData { get; set; }
    [JsonProperty]
    public int Duration { get; set; }
    [JsonProperty]
    public string? Caption { get; set; }
}

public class TelegramSendResponseDto
{
    [JsonProperty]
    public string? MessageId { get; set; }
    [JsonProperty]
    public string? ChatId { get; set; }
}