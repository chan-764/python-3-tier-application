
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QuotesWeb.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace QuotesWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public HomeController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiUrl = configuration.GetValue<string>("API_URL");
        }

        // GET: Home
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/quotes");
            if (response.IsSuccessStatusCode)
            {
                var quotes = await response.Content.ReadAsAsync<List<Quote>>();
                return View(quotes);
            }

            return View(new List<Quote>());
        }

        // POST: Home
        [HttpPost]
        public async Task<IActionResult> AddQuote(Quote newQuote)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/api/quotes", newQuote);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("Error");
        }
    }
}
