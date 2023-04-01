using Newtonsoft.Json;

public class TelegramWebhookMessageDto
{
    [JsonProperty]
    public int update_id {get; set;}
    [JsonProperty]
    public Message? message {get; set;}
}

public class Message
{
    [JsonProperty]
    public int date {get; set;}
    [JsonProperty]
    public Chat? chat {get; set;}
    [JsonProperty]
    public User? from {get; set;}
    [JsonProperty]
    public int message_id {get; set;}
    [JsonProperty]
    public string? text {get; set;}
    [JsonProperty]
    public Reply? reply_to_message {get; set;}
}

public class Reply
{
    [JsonProperty]
    public int date {get; set;}
    [JsonProperty]
    public Chat? chat {get; set;}
    [JsonProperty]
    public int message_id {get; set;}
    [JsonProperty]
    public string? text {get; set;}
}

public class User 
{
    [JsonProperty]
    public long id {get; set;}
    [JsonProperty]
    public string? first_name {get; set;}
    [JsonProperty]
    public string? last_name {get; set;}
    [JsonProperty]
    public string? username {get; set;}    
}

public class Chat : User
{
    [JsonProperty]
    public string? type {get; set;}
}