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
            var commandAndMessage = input.message.text.Substring(1).Split(' ', 2);
            string commandName = commandAndMessage[0];
            string message = commandAndMessage.Length > 1 ? commandAndMessage[1] : string.Empty;

            _logger.LogInformation($"#{commandName} Command received");

            Func<string, ICommand> createReplyMessageCommand = (cmdName) => new ReplyMessageCommand(Enum.Parse<ChatCommands>(cmdName),
                                                                                                    input,
                                                                                                    _logger,
                                                                                                    _serviceProvider.GetService<ITelegramMessageService>(),
                                                                                                    _serviceProvider.GetService<IOpenAiApiService>());
            var commandFactory = new Dictionary<string, Func<ICommand>>
            {
                [ChatCommands.Chat.GetStringValue()] = () => createReplyMessageCommand(ChatCommands.Chat.GetStringValue()),
                [ChatCommands.Voice.GetStringValue()] = () => createReplyMessageCommand(ChatCommands.Voice.GetStringValue()),
                [ChatCommands.ResetTopic.GetStringValue()] = () => new ResetTopicCommand(ChatCommands.ResetTopic,
                                                                                         input.message.chat.id,
                                                                                         _logger,
                                                                                         _serviceProvider.GetService<ITelegramMessageService>(),
                                                                                         _serviceProvider.GetService<IOpenAiApiService>()),
                [ChatCommands.Question.GetStringValue()] = () => new CreateCompletionCommand(ChatCommands.Question,
                                                                                             message,
                                                                                             _logger,
                                                                                             _serviceProvider.GetService<ITelegramMessageService>(),
                                                                                             _serviceProvider.GetService<IOpenAiApiService>()),
            };

            if (commandFactory.TryGetValue(commandName, out var createCommand))
                return createCommand();
            else
                throw new ArgumentException("Invalid command.");
        }
        else
        {
            var config = _serviceProvider.GetService<IConfiguration>();
            if (input.message.chat?.id > 0 ||
            input.message.text.Contains("@kkr_ai_bot") ||
            input.message.reply_to_message != null)
            {
                input.message.text = $"{input.message.reply_to_message?.text} \n {input.message.text ?? string.Empty}";
                return new ReplyMessageCommand(ChatCommands.Dm,
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