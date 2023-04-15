using OpenAI_API.Chat;

public class OpenAiCommandFactory
{
    private readonly ILogger _logger;
    private IServiceProvider _serviceProvider;
    public OpenAiCommandFactory(ILogger logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
    public ICommand CreateCommand(TelegramWebhookMessageDto input)
    {
        if (input.message.text.StartsWith("#"))
        {
            int index = input.message.text.IndexOf(' ');
            string command = string.Empty, message = string.Empty;
            if (index != -1)
            {
                command = input.message.text.Substring(0, index);
                message = input.message.text.Substring(index + 1);
            }
            else
                command = input.message.text;

            _logger.LogInformation($"{command} Command received");
            string name = command.Substring(1);
            switch (name)
            {
                case "chat":
                    return new ReplyMessageCommand(name,
                                                   input,
                                                   _logger,
                                                   _serviceProvider.GetService<ITelegramMessageService>(),
                                                   _serviceProvider.GetService<IOpenAiApiService>());
                case "resetTopic":
                    return new ResetTopicCommand(name,
                                                 input.message.chat.id,
                                                 _logger,
                                                 _serviceProvider.GetService<ITelegramMessageService>(),
                                                 _serviceProvider.GetService<IOpenAiApiService>());
                case "question":
                    return new CreateCompletionCommand(name,
                                                       message,
                                                       _logger,
                                                       _serviceProvider.GetService<ITelegramMessageService>(),
                                                       _serviceProvider.GetService<IOpenAiApiService>());
                default:
                    throw new ArgumentException("Invalid command.");
            }
        }
        else
        {
            var config = _serviceProvider.GetService<IConfiguration>();         
            if (input.message.chat?.id > 0 || 
            input.message.text.Contains("@kkr_ai_bot") || 
            input.message.reply_to_message != null)
            {
                input.message.text = $"{input.message.reply_to_message?.text} \n {input.message.text ?? string.Empty}";
                return new ReplyMessageCommand("DM",
                                                   input,
                                                   _logger,
                                                   _serviceProvider.GetService<ITelegramMessageService>(),
                                                   _serviceProvider.GetService<IOpenAiApiService>());
            }
            else
                throw new ArgumentException("Regular Non Prompt Chat");
        }
    }
}