using Newtonsoft.Json;

public class TelegramSendMessageRequestDto
{
    [JsonProperty]
    public string? chat_id {get; set;}
    [JsonProperty]
    public string? text {get; set;}
}

public class TelegramSendMessageResponseDto
{
    [JsonProperty]
    public Message? message {get; set;}
}