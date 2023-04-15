using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

public class TelegramMessageServiceV2 : ITelegramMessageService
{
    private readonly TelegramBotClient _botClient;
    private readonly ITextToSpeechService _textToSpeechService;

    public TelegramMessageServiceV2(IConfiguration configuration, ITextToSpeechService textToSpeechService)
    {
        var telegramBotToken = configuration["TelegramBotToken"];
        _botClient = new TelegramBotClient(telegramBotToken);
        
        _textToSpeechService = textToSpeechService;
    }

    public async Task<TelegramSendResponseDto> SendMessageAsync(TelegramSendMessageRequestDto request)
    {
        var message = await _botClient.SendTextMessageAsync(request.ChatId, request.Text);

        return new TelegramSendResponseDto
        {
            MessageId = message.MessageId.ToString(),
            ChatId = message.Chat.Id.ToString()
        };
    }

    public async Task<TelegramSendResponseDto> SendVoiceAsync(TelegramSendMessageRequestDto request)
    {
        var sendVoiceRequest = new TelegramSendVoiceRequestDto()
        {
            ChatId = request.ChatId,
            VoiceData = await _textToSpeechService.GenerateAudioByteAsync(request.Text)
        };
        return await SendVoiceAsync(sendVoiceRequest);
    }

    public async Task<TelegramSendResponseDto> SendVoiceAsync(TelegramSendVoiceRequestDto request)
    {
        using var stream = new MemoryStream(request.VoiceData);
        var voice = new InputOnlineFile(stream, $"{request.VoiceName}.ogg");

        var sendVoiceRequest = new SendVoiceRequest(request.ChatId, voice)
        {
            Caption = request.Caption,
            Duration = request.Duration
        };

        var message = await _botClient.SendVoiceAsync(chatId: sendVoiceRequest.ChatId,
                                                      voice: sendVoiceRequest.Voice,
                                                      caption: sendVoiceRequest.Caption,
                                                      duration: sendVoiceRequest.Duration);

        return new TelegramSendResponseDto
        {
            MessageId = message.MessageId.ToString(),
            ChatId = message.Chat.Id.ToString()
        };
    }
}