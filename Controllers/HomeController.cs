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
        private readonly ITelegramMessageService _telegramMessageService;

        public HomeController(ILogger<HomeController> logger, ITelegramMessageService telegramMessageService)
        {
            _logger = logger;
            _telegramMessageService = telegramMessageService;
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
            _logger.Log(LogLevel.Information, "Message Received", value);

            return Ok(_telegramMessageService.SendMessageAsync(new TelegramSendMessageRequestDto
            {
                chat_id = value.message.chat.id.ToString(),
                text = value.message.text
            }));
        }
    }
}
