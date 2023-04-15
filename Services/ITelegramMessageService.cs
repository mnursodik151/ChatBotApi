public interface ITelegramMessageService 
{
    public Task<TelegramSendResponseDto> SendMessageAsync(TelegramSendMessageRequestDto requestDto);
}