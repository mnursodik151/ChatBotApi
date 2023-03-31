
using OpenAI_API.Completions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ITelegramMessageService, TelegramMessageService>(client =>
{
    client.BaseAddress = new Uri("https://api.telegram.org/");
});

builder.Services.AddScoped<IOpenAiApiService, OpenAiApiService>();
builder.Services.AddScoped<IObservable<CompletionResult>, OpenAiCompletionObservable>();
builder.Services.AddSingleton<IObservable<TelegramWebhookMessageDto>, OpenAiChatObservable>();

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

