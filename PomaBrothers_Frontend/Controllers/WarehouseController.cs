using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PomaBrothers_Frontend.Models;
using System.Net.Http.Headers;

namespace PomaBrothers_Frontend.Controllers
{
    public class WarehouseController : Controller
    {
        private HttpClient httpClient = new();

        public WarehouseController() 
        {
            httpClient.BaseAddress = new Uri("http://localhost:5164/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IActionResult> Index()
        {
            var list = await GetWarehouses();
            ViewBag.Warehouses = list;  
            return View();
        }

        public async Task<List<Warehouse>> GetWarehouses()
        {
            HttpResponseMessage request = await httpClient.GetAsync("Warehouse/GetMany");
            request.EnsureSuccessStatusCode();
            var serialize = request.Content.ReadAsStringAsync().Result;
            var warehouses = JsonConvert.DeserializeObject<List<Warehouse>>(serialize);
            return warehouses;
        }

        public async Task<IActionResult> Delete([FromQuery]int id)
        {
            HttpResponseMessage request = await httpClient.DeleteAsync($"Warehouse/Remove/{id}");
            request.EnsureSuccessStatusCode();
            return NoContent();
        }

        #region Sections
        public async Task<ActionResult> ShowModelsAvailable([FromQuery]int id)
        {
            HttpResponseMessage request = await httpClient.GetAsync($"Warehouse/GetContentWarehouse/{id}");
            request.EnsureSuccessStatusCode();
            var serialize = request.Content.ReadAsStringAsync().Result;
            var models = JsonConvert.DeserializeObject<List<Section>>(serialize);
            return Json(models);
        }

        public async Task<ActionResult> ShowItemsAvailable([FromRoute]int id)
        {
            HttpResponseMessage request = await httpClient.GetAsync($"Warehouse/GetItemsWarehouse/{id}");
            request.EnsureSuccessStatusCode();
            var serialize = request.Content.ReadAsStringAsync().Result;
            var items = JsonConvert.DeserializeObject<List<Item>>(serialize);
            return Json(items);
        }
        #endregion
    }
}
