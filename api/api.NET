using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// MongoDB Configuration
var mongoUri = Environment.GetEnvironmentVariable("MONGO_URI") ?? "mongodb://mongodb-svc:27017/";
var client = new MongoClient(mongoUri);
var database = client.GetDatabase("quotesdb");
var quotesCollection = database.GetCollection<Quote>("quotes");

builder.Services.AddSingleton(quotesCollection);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowAll");

// Get all quotes
app.MapGet("/api/quotes", async (IMongoCollection<Quote> collection) =>
{
    var quotes = await collection.Find(_ => true).ToListAsync();
    return Results.Ok(quotes);
});

// Add a new quote
app.MapPost("/api/quotes", async (IMongoCollection<Quote> collection, Quote quote) =>
{
    if (string.IsNullOrEmpty(quote.QuoteText) || string.IsNullOrEmpty(quote.Author))
    {
        return Results.BadRequest("Both 'quote' and 'author' are required.");
    }

    await collection.InsertOneAsync(quote);
    return Results.Created($"/api/quotes/{quote.Id}", quote);
});

// Health check
app.MapGet("/ok", () => Results.Ok("OK"));

app.Run();

public class Quote
{
    public string Id { get; set; }
    public string QuoteText { get; set; }
    public string Author { get; set; }
}
