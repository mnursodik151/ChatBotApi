using OpenAI_API;
using OpenAI_API.Chat;

public class OpenAiChatObserver : OpenAiObserver, IObserver<TelegramWebhookMessageDto>
{
    private Conversation _chat;
    private ILogger _logger;

    public OpenAiChatObserver(OpenAIAPI api, ITelegramMessageService telegramMessageService, string chat_id, ILogger logger) : base(telegramMessageService, chat_id)
    {
        _logger = logger;
        _chat = api.Chat.CreateConversation();

        // give instruction as System
        _chat.AppendSystemMessage("Kamu adalah seorang ketua organisasi karang taruna, sebagai anak muda kamu selalu menjawab pertanyaan dengan semangat namun santai dan sesekali suka mengeluarkan lelucon bapak-bapak");
        _chat.AppendSystemMessage("You are a close friend who like to crack some dad jokes every now and then");

        // give instruction as System
        _chat.AppendUserInput("Hari ini enaknya makan apa ya?");
        _chat.AppendExampleChatbotOutput("Wah, kalau siang gini enaknya nasi sama ayam. Eh jangan ding nanti nasinya dihabisin sama ayam");

        _chat.AppendUserInput("Why are novels usually a certain pages long?");
        _chat.AppendExampleChatbotOutput("it's a long story lol, but basically many publishers and literary agents consider 80,000 to 100,000 words to be the standard for adult novels1. This word count usually translates to around 300 to 350 pages depending on font size and other formatting factors.");
    }

    public string GetChatId() => _chatId;

    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        _logger.LogError(error, "Error When Executing Chat");
    }

    public async void OnNext(TelegramWebhookMessageDto value)
    {
        try
        {
            _logger.LogInformation("Starting conversation");
            // now let's ask it a question'
            _chat.AppendUserInput(value.message?.text);
            // and get the response
            string response = await _chat.GetResponseFromChatbot();
            //send the response back to chat
            await _telegramMessageService.SendMessageAsync(new TelegramSendMessageRequestDto()
            {
                chat_id = _chatId,
                text = response
            });
        }
        catch (Exception ex)
        {
            this.OnError(ex);
        }
    }
}