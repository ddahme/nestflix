using Api.Database;
using Api.Endpoints;
using Api.Repositories;
using Api.Services;
using Api.Swagger;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.EntityFrameworkCore;
using OpenAI.Chat;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddProblemDetails();

builder.Services.ConfigureFullSwaggerConfig();

builder.Services.AddDbContextFactory<NestflixDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});

AzureKeyCredential credential = new (builder.Configuration["AZURE_OPENAI_KEY"] ?? throw new InvalidOperationException("Missing AZURE_OPENAI_KEY"));
AzureOpenAIClient azureClient = new (new Uri(builder.Configuration["AZURE_OPENAI_ENDPOINT"] ?? throw new InvalidOperationException("Missing AZURE_OPENAI_ENDPOINT")), credential);
ChatClient chatClient = azureClient.GetChatClient("gpt-4");
builder.Services.AddSingleton(chatClient);

builder.Services.AddSingleton<IAiService, AiService>();
builder.Services.AddScoped<ITweetRepository, TweetRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.MapAiEndpoints();

await app.RunAsync();
