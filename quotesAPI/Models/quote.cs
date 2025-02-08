using System.ComponentModel.DataAnnotations.Schema;

namespace QuotesApi.Models
{
    [Table("quotes")]  // Ensure it matches the actual MySQL table name
    public class Quote
    {
        public int Id { get; set; }
        public string QuoteText { get; set; }
        public string Author { get; set; }
    }
}
