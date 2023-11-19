using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PomaBrothers_Frontend.Models;
using System.Net.Http.Headers;

namespace PomaBrothers_Frontend.Controllers
{
    public class CategoryController : Controller
    {
        private HttpClient httpClient = new();

        public CategoryController()
        {
            httpClient.BaseAddress = new Uri("http://localhost:5164/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.GetCategories = await GetCategoriesAsync();
            return View();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            HttpResponseMessage request = await httpClient.GetAsync("Category/GetMany");
            request.EnsureSuccessStatusCode();
            string json = request.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Category>>(json);
        }

        public async Task<IActionResult> ItemsByCategories([FromRoute]int id, int page = 1, int pageSize = 6)
        {
            var items = await GetItemsCategoryAsync(id);
            if(items != null)
            {
                int totalItems = items.Count;

                var paginatedData = items.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.Data = paginatedData;
                ViewBag.CurrentPage = page;
                ViewBag.CategoryId = id;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                return View();
            }
            return View();
        }

        public async Task<List<Item>> GetItemsCategoryAsync(int id)
        {
            HttpResponseMessage request = await httpClient.GetAsync($"Category/FilterByCategory/{id}");
            if (request.IsSuccessStatusCode)
            {
                string json = request.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Item>>(json);
            }
            return null!;
        }
    }
}
