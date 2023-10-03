using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PomaBrothers_Frontend.Models;
using PomaBrothers_Frontend.Models.DTOModels;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PomaBrothers_Frontend.Controllers
{
    public class DeliveryController : Controller
    {
        private HttpClient httpClient = new();

        public DeliveryController()
        {
            httpClient.BaseAddress = new Uri("http://localhost:5164/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoicesWithDetails()
        {
            var invoideDetails = new List<object>();

            HttpResponseMessage request = await httpClient.GetAsync("Delivery/GetMany");
            if (request.IsSuccessStatusCode)
            {
                var json = request.Content.ReadAsStringAsync().Result;
                var getItem = JsonConvert.DeserializeObject<List<object>>(json);
                return Ok(getItem);
            }
            return null!;
        }

        public async Task<IActionResult> New()
        {
            ViewBag.Categories = await GetCategoriesAsync();
            ViewBag.Suppliers = await GetSuppliersAsync();
            ViewBag.Warehouses = await GetWarehousesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> New([FromBody]DeliveryDTO objDelivery)
        {
            try
            {
                HttpResponseMessage request = await httpClient.PostAsJsonAsync("Delivery/New", objDelivery);
                if (request.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Item");
                }
                return View();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Get categories & suppliers
        public async Task<List<Category>> GetCategoriesAsync()
        {
            HttpResponseMessage request = await httpClient.GetAsync("Category/GetMany");
            if (request.IsSuccessStatusCode)
            {
                var serializeList = request.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Category>>(serializeList);
            }
            return null!;
        }

        public async Task<List<Supplier>> GetSuppliersAsync()
        {
            HttpResponseMessage request = await httpClient.GetAsync("Supplier/GetMany");
            if (request.IsSuccessStatusCode)
            {
                var serializeList = request.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Supplier>>(serializeList);
            }
            return null!;
        }

        public async Task<List<Warehouse>> GetWarehousesAsync()
        {
            HttpResponseMessage request = await httpClient.GetAsync("Warehouse/GetMany");
            request.EnsureSuccessStatusCode();
            var serialize = request.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Warehouse>>(serialize);
        }
        #endregion
    }
}
