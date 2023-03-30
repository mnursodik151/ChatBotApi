using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITelegramMessageService? _telegramMessageService;

        public HomeController(ILogger<HomeController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _telegramMessageService = serviceProvider.GetService<ITelegramMessageService>();
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
            var request = new TelegramSendMessageRequestDto
            {
                chat_id = value.message?.chat?.id.ToString(),
                text = value.message?.text
            };
            _logger.Log(LogLevel.Information, $"Message Received, chatID = {request.chat_id}; text = {request.text}");
            var result = await _telegramMessageService.SendMessageAsync(request);
            return Ok(result);
        }
    }
}
