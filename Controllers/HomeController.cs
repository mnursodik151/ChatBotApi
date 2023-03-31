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
        public async Task<IActionResult> Post([FromBody] TelegramWebhookMessageDto value)
        {
            try
            {
                var observable = _openAiApiService?.GetCompletionObservable();
                var observer = new OpenAiCompletionObserver(_telegramMessageService, value.message.chat.id.ToString());
                var subscription = observable?.Subscribe(observer);

                await _openAiApiService.StreamCompletionEnumerableAsync(new CompletionRequest(
                    value.message.text, OpenAI_API.Models.Model.DavinciText, 255, 0.6));
                subscription?.Dispose();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Internal Server Error", ex);
                var result = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                await _telegramMessageService.SendMessageAsync(new TelegramSendMessageRequestDto()
                {
                    chat_id = value.message.chat.id.ToString(),
                    text = result.ToString()
                });
                return result;
            }
        }
    }
}
