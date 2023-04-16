
using OpenAI_API.Completions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddHttpClient<ITelegramMessageService, TelegramMessageService>(client =>
// {
//     client.BaseAddress = new Uri("https://api.telegram.org/");
// });

builder.Services.AddScoped<ITelegramMessageService, TelegramMessageServiceV2>();
builder.Services.AddScoped<ITextToSpeechService, GCPTextToSpeechService>();
builder.Services.AddScoped<IOpenAiApiService, OpenAiApiService>();
builder.Services.AddScoped<IObservable<CompletionResult>, OpenAiCompletionObservable>();
builder.Services.AddSingleton<IObservable<TelegramSendMessageRequestDto>, OpenAiChatObservable>();
builder.Services.AddSingleton<TextLanguageDetectionUtil>();

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

