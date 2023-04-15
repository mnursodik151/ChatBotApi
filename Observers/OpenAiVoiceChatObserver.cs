using OpenAI_API;
using OpenAI_API.Chat;

public class OpenAiVoiceChatObserver : OpenAiObserver
{
    private Conversation _chat;
    private ILogger _logger;

    public OpenAiVoiceChatObserver(OpenAIAPI api, ITelegramMessageService telegramMessageService, ILogger logger, long chat_id, ChatCommands command) 
    : base(telegramMessageService, chat_id, command)
    {
        _logger = logger;
        _chat = api.Chat.CreateConversation();

        // give instruction as System
        _chat.AppendSystemMessage("You are a close friend who like to crack some dad jokes every now and then");
        _chat.AppendSystemMessage("Your name is Rangga, you are a telegram bot with user name Ketua Karang Taruna RT 03 and you are running on an AWS EC2 instance using chatGPT api");

        // give sample conversation
        _chat.AppendUserInput("I am practicing a casual conversation with you, you have to reply as if you are talking to me verbally, and casualy like a close friend in real life, if understood say yes");
        _chat.AppendExampleChatbotOutput("Of course man, I'm always here to talk to you 'bot everything");
    }

    public override void OnError(Exception error)
    {
        _logger.LogError(error, "Error When Executing Chat");
    }

    public override async void OnNext(TelegramSendMessageRequestDto value)
    {
        try
        {
            _logger.LogInformation("Starting conversation");
            // now let's ask it a question'
            _chat.AppendUserInput(value.Text);
            // and get the response
            string response = await _chat.GetResponseFromChatbot();
            //send the response back to chat
            await _telegramMessageService.SendVoiceAsync(new TelegramSendMessageRequestDto()
            {
                ChatId = _chatId,
                Text = response
            });
        }
        catch (Exception ex)
        {
            this.OnError(ex);
        }
    }
}