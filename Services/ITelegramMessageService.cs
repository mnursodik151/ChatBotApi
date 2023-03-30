public interface ITelegramMessageService 
{
    public Task<TelegramSendMessageResponseDto> SendMessageAsync(TelegramSendMessageRequestDto requestDto);
}