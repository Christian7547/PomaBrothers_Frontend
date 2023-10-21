using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PomaBrothers_Frontend.Models;
using System.Net.Http.Headers;

namespace PomaBrothers_Frontend.Controllers
{
    public class SaleController : Controller
    {
        private HttpClient _httpClient = new();

        public SaleController()
        {
            _httpClient.BaseAddress = new Uri("http://localhost:5164/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<List<Sale>> GetSales()
        {
            HttpResponseMessage request = await _httpClient.GetAsync("Sale/GetMany");
            request.EnsureSuccessStatusCode();
            string json = request.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Sale>>(json);
        }
    }
}
