using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API.Completions;

namespace ChatBotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITelegramMessageService? _telegramMessageService;
        private readonly IOpenAiApiService? _openAiApiService;
        private readonly OpenAiCommandFactory _openAiCommandFactory;

        public HomeController(ILogger<HomeController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _telegramMessageService = serviceProvider.GetService<ITelegramMessageService>();
            _openAiApiService = serviceProvider.GetService<IOpenAiApiService>();
            _openAiCommandFactory = new OpenAiCommandFactory(logger, serviceProvider);
        }

        // GET: api/Home
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Hello", "World" };
        }

        // GET: api/Home/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return $"Helo {id} World";
        }

        // POST: api/Home
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TelegramWebhookMessageDto value)
        {
            try
            {
                if (value?.message == null)
                    throw new Exception("Undandled Webhook Call format: Most likely state update");   
                if(!value.message.text.StartsWith("#"))
                    throw new NotImplementedException("Not A Prompt");

                var convo = _openAiApiService?.TryAddConversation(_telegramMessageService, value.message.chat.id.ToString());
                if (convo == null)
                    throw new Exception("Failed to get conversation from list");

                var command = _openAiCommandFactory.CreateCommand(value);
                await command.ExecuteAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Internal Server Error : {ex.Message}");
                return Ok("ok with caveats");
            }
        }
    }
}
