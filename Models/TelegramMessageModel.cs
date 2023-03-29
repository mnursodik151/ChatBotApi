public class TelegramMessage
{
    public int update_id {get; set;}
    public Message? message {get; set;}
}

public class Message
{
    public int date {get; set;}
    public Sender? chat {get; set;}
    public int message_id {get; set;}
    public Sender? from {get; set;}
    public string? text {get; set;}
}

public class Sender 
{
    public int id {get; set;}
    public string? first_name {get; set;}
    public string? last_name {get; set;}
    public string? username {get; set;}
    public string? type {get; set;}
}

