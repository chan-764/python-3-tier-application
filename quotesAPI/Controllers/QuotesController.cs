using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuotesApi.Data;
using QuotesApi.Models;

namespace QuotesApi.Controllers
{
    [Route("api/quotes")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly QuotesDbContext _context;

        public QuotesController(QuotesDbContext context)
        {
            _context = context;
        }

        // GET: /api/quotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quote>>> GetQuotes()
        {
            var quotes = await _context.Quotes.ToListAsync();
            return Ok(quotes);
        }

        // GET: /api/quotes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Quote>> GetQuoteById(int id)
        {
            var quote = await _context.Quotes.FindAsync(id);
            if (quote == null)
            {
                return NotFound(new { message = "Quote not found." });
            }
            return Ok(quote);
        }

        // POST: /api/quotes
        [HttpPost]
        public async Task<ActionResult<Quote>> PostQuote([FromBody] Quote quote)
        {
            if (quote == null || string.IsNullOrWhiteSpace(quote.QuoteText) || string.IsNullOrWhiteSpace(quote.Author))
            {
                return BadRequest(new { message = "Both quote text and author fields are required." });
            }

            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetQuoteById), new { id = quote.Id }, quote);
        }
    }
}
