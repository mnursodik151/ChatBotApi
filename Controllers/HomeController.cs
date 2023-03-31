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

        public HomeController(ILogger<HomeController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _telegramMessageService = serviceProvider.GetService<ITelegramMessageService>();
            _openAiApiService = serviceProvider.GetService<IOpenAiApiService>();
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
        public IActionResult Post([FromBody] TelegramWebhookMessageDto value)
        {
            try
            {
                if (value?.message == null)
                    throw new Exception("Undandled Webhook Call format: Most likely state update");                

                var convo = _openAiApiService?.TryAddConversation(_telegramMessageService, value.message.chat.id.ToString());
                if (convo == null)
                    throw new Exception("Failed to get conversation from list");

                // if(value.message.text.StartsWith('/'))
                // {
                    
                // }

                _openAiApiService?.AddChatMessage(value);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal Server Error", ex);
                var result = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                return result;
            }
        }
    }
}
