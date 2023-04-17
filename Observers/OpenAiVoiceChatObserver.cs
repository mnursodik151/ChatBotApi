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
        _chat.AppendUserInput("can you reply in gcp SSML format so that it can be easily converted into text-to-speech");
        _chat.AppendExampleChatbotOutput("Certainly! I can respond using Google Cloud Platform's Speech Synthesis Markup Language (SSML) format. Here's an example: <speak>Hello, how are you today?</speak> This SSML code will produce the spoken output 'Hello, how are you today?' when passed through a text-to-speech engine that supports SSML. Let me know if you have any further questions or if there's anything else I can assist you with.");
        _chat.AppendUserInput("from now on make your reply in gcp SSML format");
        _chat.AppendExampleChatbotOutput("Sure, I can reply in GCP SSML format. From now on, all my responses will be in SSML format to make it easier for you to convert them to speech.");
        _chat.AppendUserInput("what are the tags that can be used in SSML to control intonation and tone?");
        _chat.AppendExampleChatbotOutput("There are several tags that can be used in SSML to control intonation and tone. Here are some of the most commonly used tags:\n<prosody>: This tag is used to adjust the volume, pitch, and rate of speech. It can be used to emphasize certain words or phrases, or to create a sense of urgency or excitement.\n<emphasis>: This tag is used to emphasize a particular word or phrase. It can be used to create a sense of importance or to draw attention to a specific point.\n<break>: This tag is used to insert a pause or silence in the speech. It can be used to create a sense of anticipation or to mark a natural break in the text.\n<speak>: This tag is used to indicate the start and end of the speech. It can be used to apply certain effects to the entire text, such as adjusting the overall volume or pitch.\n<say-as>: This tag is used to control the pronunciation and format of numbers, dates, and other types of data. It can be used to ensure that the text is read correctly and clearly.\nThese tags can be combined and nested to create complex effects and adjust the overall tone and style of the speech.");
        _chat.AppendUserInput("apply those to your reply");
        _chat.AppendExampleChatbotOutput("<speak>\nCertainly! In my previous response, I used a neutral tone with a moderate pace and pitch. I <emphasis level=\"moderate\">emphasized</emphasis> the tags <prosody>, <emphasis>, <break>, <speak>, and <say-as> to highlight their functions and demonstrate how they can be used in SSML. I also <prosody rate=\"slow\" pitch=\"-2st\">adjusted my tone and pace slightly</prosody> when discussing the different tags, to make the explanation more clear and engaging. <break time=\"500ms\"/> Overall, my goal was to provide a helpful and informative response in a clear and natural-sounding voice. Let me know if you have any further questions!\n</speak>");
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