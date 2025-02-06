using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuotesApi.Data;
using QuotesApi.Models;

namespace QuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly QuotesDbContext _context;

        public QuotesController(QuotesDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quote>>> GetQuotes()
        {
            return await _context.Quotes.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Quote>> PostQuote(Quote quote)
        {
            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetQuotes), new { id = quote.Id }, quote);
        }
    }
}
