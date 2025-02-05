
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

[Route("api/[controller]")]
[ApiController]
public class QuotesController : ControllerBase
{
    private readonly string _connectionString;

    public QuotesController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    [HttpGet]
    public IActionResult GetQuotes()
    {
        var quotes = new List<object>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM quotes", connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    quotes.Add(new
                    {
                        Id = reader["id"],
                        Quote = reader["quote"],
                        Author = reader["author"]
                    });
                }
            }
        }
        return Ok(quotes);
    }

    [HttpPost]
    public IActionResult AddQuote([FromBody] dynamic quote)
    {
        var content = quote.quote;
        var author = quote.author;
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("INSERT INTO quotes (quote, author) VALUES (@quote, @author)", connection);
            command.Parameters.AddWithValue("@quote", content);
            command.Parameters.AddWithValue("@author", author);
            command.ExecuteNonQuery();
        }
        return CreatedAtAction(nameof(GetQuotes), new { quote = content, author = author });
    }

    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok("OK");
    }
}
