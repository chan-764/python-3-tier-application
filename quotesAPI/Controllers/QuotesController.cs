using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly string connectionString = "server=db;port=3306;database=quotesdb;user=user;password=password";

        // GET: api/quotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quote>>> GetQuotes()
        {
            var quotes = new List<Quote>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var cmd = new MySqlCommand("SELECT * FROM quotes", connection))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        quotes.Add(new Quote
                        {
                            Id = reader.GetInt32("id"),
                            QuoteText = reader.GetString("quote"),
                            Author = reader.GetString("author")
                        });
                    }
                }
            }

            return Ok(quotes);
        }

        // POST: api/quotes
        [HttpPost]
        public async Task<ActionResult<Quote>> AddQuote([FromBody] Quote newQuote)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var query = "INSERT INTO quotes (quote, author) VALUES (@quote, @author)";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@quote", newQuote.QuoteText);
                    cmd.Parameters.AddWithValue("@author", newQuote.Author);
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return CreatedAtAction(nameof(GetQuotes), new { id = newQuote.Id }, newQuote);
        }
    }

    public class Quote
    {
        public int Id { get; set; }
        public string QuoteText { get; set; }
        public string Author { get; set; }
    }
}
