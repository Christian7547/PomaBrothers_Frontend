using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using PomaBrothers_Frontend.Models;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuestPDF.Helpers;
using PomaBrothers_Frontend.Models.DTOModels;

namespace PomaBrothers_Frontend.Controllers
{
    public class SupplierController : Controller
    {
        private HttpClient httpClient = new();

        public SupplierController()
        {
            httpClient.BaseAddress = new Uri("http://localhost:5164/"); 
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<ActionResult> Index(int page = 1, int pageSize = 6)
        {
            var getSupplier = await GetSupplierAsync();
            var query = getSupplier.Select(item => new
            {
                _SupID = item.Id,
                _BussinesName = item.BussinesName,
                _Manager = item.Manager,
                _Phone = item.Phone,
                _Ci = item.Ci,
                _Address = item.Address
            }).ToList();
            int totalSuppliers = query.Count;
            var paginatedData = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.Data = paginatedData;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalSuppliers / pageSize);
            return View();
        }

        public async Task<List<Supplier>> GetSupplierAsync()
        {
            HttpResponseMessage request = await httpClient.GetAsync("Supplier/GetMany");
            if (request.IsSuccessStatusCode)
            {
                var serializeList = request.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Supplier>>(serializeList);
            }
            return null!;
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.Data = await GetSupplierAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            try
            {
                HttpResponseMessage request = await httpClient.PostAsJsonAsync("Supplier/NewSupplier", supplier);
                request.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Supplier", request.Headers.Location);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            Supplier supplier = await GetSupAsync(id);
            ViewBag.Data = await GetSupplierAsync();
            return View(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Supplier supplier)
        {
            try
            {
                HttpResponseMessage request = await httpClient.PutAsJsonAsync("Supplier/EditSupplier", supplier);
                request.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Supplier");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Supplier> GetSupAsync(int id)
        {
            HttpResponseMessage request = await httpClient.GetAsync($"Supplier/GetOne/{id}");
            if (request.IsSuccessStatusCode)
            {
                var serializeObject = request.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Supplier>(serializeObject);
            }
            return null!;
        }

        [HttpPost]
        public async Task<ActionResult<Supplier>> GetSupplier([FromQuery] int id)
        {
            var sup = await GetSupAsync(id);
            return Ok(sup);
        }

        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            try
            {
                HttpResponseMessage request = await httpClient.DeleteAsync($"Supplier/RemoveSupplier/{id}");
                request.EnsureSuccessStatusCode();
                return NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<ActionResult> SearchSupplier(string likeSupplier)
        {
            HttpResponseMessage request = await httpClient.GetAsync($"Supplier/SearchSupplier/{likeSupplier}");
            var serializeList = request.Content.ReadAsStringAsync().Result;
            List<SupplierSearchDTO> list = JsonConvert.DeserializeObject<List<SupplierSearchDTO>>(serializeList);
            return Json(list);
        }
    }
}
