using Api.Database;
using Api.Endpoints;
using Api.Repositories;
using Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContextFactory<NestflixDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});

builder.Services.AddScoped<ITweetRepository, TweetRepository>();
builder.Services.AddScoped<IBoxRepository, BoxRepository>();
builder.Services.AddScoped<IBoxService, BoxService>();
builder.Services.AddScoped<ITweetService, TweetService>();

var app = builder.Build();

app.MapBoxEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

await app.RunAsync();
