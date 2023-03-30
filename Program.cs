var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

// Retrieve the Telegram bot token from an environment variable
var telegramBotToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");

builder.Services.AddHttpClient<ITelegramMessageService>(client =>
{
    client.BaseAddress = new Uri("https://api.telegram.org/");
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
}).AddTypedClient(client => new TelegramMessageService(client))
  .ConfigureHttpClient(client =>
  {
      client.DefaultRequestHeaders.Add("Authorization", $"Bearer {telegramBotToken}");
  });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
