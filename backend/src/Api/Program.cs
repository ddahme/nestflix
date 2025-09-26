using Api.Clients;
using Api.Database;
using Api.Endpoints;
using Api.Repositories;
using Api.Services;
using Api.Swagger;
using Azure;
using Azure.AI.OpenAI;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using OpenAI.Chat;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddProblemDetails();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.ConfigureFullSwaggerConfig();

builder.Services.AddDbContextFactory<NestflixDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"), npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        npgsqlOptions.CommandTimeout(60);
        npgsqlOptions.UseNetTopologySuite();
    });
});

builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration.GetValue<string>($"AZURE_BLOB_STORAGE_CONNECTION") ?? throw new InvalidOperationException("AZURE_BLOB_STORAGE_CONNECTION is not set!"));
});

builder.Services.AddSingleton(provider =>
{
    var blobServiceClient = provider.GetRequiredService<BlobServiceClient>();
    var containerName = builder.Configuration.GetValue<string>("STORAGE_CONTAINER") ?? throw new InvalidOperationException("STORAGE_CONTAINER is not set!");
    BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
    blobContainerClient.CreateIfNotExists();
    return blobContainerClient;
});

AzureKeyCredential credential = new(builder.Configuration["AZURE_OPENAI_KEY"] ?? throw new InvalidOperationException("Missing AZURE_OPENAI_KEY"));
AzureOpenAIClient azureClient = new(new Uri(builder.Configuration["AZURE_OPENAI_ENDPOINT"] ?? throw new InvalidOperationException("Missing AZURE_OPENAI_ENDPOINT")), credential);
ChatClient chatClient = azureClient.GetChatClient("gpt-4");
builder.Services.AddSingleton(chatClient);

builder.Services.AddSingleton<IAiService, AiService>();
builder.Services.AddScoped<ITweetRepository, TweetRepository>();
builder.Services.AddScoped<IBoxRepository, BoxRepository>();
builder.Services.AddScoped<IBlobClient, AzureBlobClient>();
builder.Services.AddScoped<IBoxService, BoxService>();
builder.Services.AddScoped<ITweetService, TweetService>();

var app = builder.Build();

app.MapBoxEndpoints();
app.MapTweetEndpoints();
app.MapAiEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseExceptionHandler();

// Migrate the database
using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<NestflixDbContext>>();
    await using var dbContext = await factory.CreateDbContextAsync();
    await dbContext.Database.MigrateAsync();
}

await app.RunAsync();
