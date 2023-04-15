public interface ITelegramMessageService 
{
    public Task<TelegramSendResponseDto> SendMessageAsync(TelegramSendMessageRequestDto requestDto);
    public Task<TelegramSendResponseDto> SendVoiceAsync(TelegramSendVoiceRequestDto voiceRequest);
    public Task<TelegramSendResponseDto> SendVoiceAsync(TelegramSendMessageRequestDto messageRequest);
}