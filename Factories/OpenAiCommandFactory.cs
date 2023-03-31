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
        int index = input.message.text.IndexOf(' ');
        string command = string.Empty, message = string.Empty;
        if (index != -1)
        {
            command = input.message.text.Substring(0, index);
            message = input.message.text.Substring(index + 1);
        }
        _logger.LogInformation($"{command} Command received");
        if (command.StartsWith("#"))
        {
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
                                                 input.message.chat.id.ToString(),
                                                 _logger,
                                                 _serviceProvider.GetService<ITelegramMessageService>(),
                                                 _serviceProvider.GetService<IOpenAiApiService>());
                default:
                    throw new ArgumentException("Invalid command.");
            }
        }
        else
        {
            throw new NotImplementedException("Regular Non Prompt Chat");
        }
    }
}