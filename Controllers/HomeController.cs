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

        public HomeController(ILogger<HomeController> logger) 
        {
            _logger = logger;
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
        public IActionResult Post([FromBody] TelegramMessage value)
        {
            _logger.Log(LogLevel.Information, "Message Received");
            return Ok(value);
        }
    }
}
