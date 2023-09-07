using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PomaBrothers_Frontend.Models;
using System.Net.Http.Headers;

namespace PomaBrothers_Frontend.Controllers
{
    public class ItemController : Controller
    {
        private HttpClient httpClient = new();

        public ItemController()
        {
            httpClient.BaseAddress = new Uri("http://localhost:5164/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ActionResult> Index()
        {
            var getItems = await GetItemsAsync();
            var getModels = await GetModelsAsync();
            var getCategories = await GetCategoriesAsync();
            var query = getItems.Join(getModels, i => i.ModelId, m => m.Id,
                (i, m) => new
                {
                    _Name = i.Name,
                    _Price = i.Price,
                    _Color = i.Color,
                    _Serie = i.Serie,
                    _Marker = m.Marker,
                    ItemID = i.Id,
                    ModelID = m.Id
                }).ToList();
            ViewBag.Data = query;
            ViewBag.Category = getCategories;
            return View();
        }

        public async Task<List<Item>> GetItemsAsync() //Get All
        {
            HttpResponseMessage request = await httpClient.GetAsync("Item/GetMany");
            if (request.IsSuccessStatusCode)
            {
                var serializeList = request.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Item>>(serializeList);
            }
            return null!;
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.Data = await GetCategoriesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            try
            {
                HttpResponseMessage request = await httpClient.PostAsJsonAsync("Item/New", item);
                request.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Item", request.Headers.Location);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            Item item = await GetItemAsync(id);
            ViewBag.Data = await GetCategoriesAsync();
            return View(item);
        }
        #region Modify Item
        [HttpPost]
        public async Task<IActionResult> Edit(Item item)
        {
            try
            {
                HttpResponseMessage request = await httpClient.PutAsJsonAsync("Item/Edit", item);
                request.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Item");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ActionResult> GetModel(int id)
        {
            HttpResponseMessage request = await httpClient.GetAsync($"ItemModel/GetOne/{id}");
            request.EnsureSuccessStatusCode();
            var serializeModel = request.Content.ReadAsStringAsync().Result;
            var getModel = JsonConvert.DeserializeObject<ItemModel>(serializeModel);
            return Json(getModel);
        }
        #endregion
        public async Task<Item> GetItemAsync(int id) //Get One
        {
            HttpResponseMessage request = await httpClient.GetAsync($"Item/GetOne/{id}");
            if (request.IsSuccessStatusCode)
            {
                var serializeItem = request.Content.ReadAsStringAsync().Result;
                var getItem =  JsonConvert.DeserializeObject<Item>(serializeItem);

                HttpResponseMessage getModel = await httpClient.GetAsync($"ItemModel/GetOne/{getItem.ModelId}");
                var serializeModel = getModel.Content.ReadAsStringAsync().Result;
                var deserializeModel = JsonConvert.DeserializeObject<ItemModel>(serializeModel);

                getItem.ItemModel = deserializeModel;
                return getItem;
            }
            return null!;
        }

        [HttpPost]
        public async Task<ActionResult<Item>> GetItem([FromQuery]int id) //This method obtains the id from the interface to send it to the method that connects to the API
        {
            var item = await GetItemAsync(id);
            return Ok(item);
        }

        public async Task<IActionResult> Delete([FromQuery]int id)
        {
            HttpResponseMessage request = await httpClient.DeleteAsync($"Item/Remove/{id}");
            request.EnsureSuccessStatusCode();
            return NoContent();
        }

        #region GetInfo
        public async Task<List<ItemModel>> GetModelsAsync()
        {
            HttpResponseMessage request = await httpClient.GetAsync("ItemModel/GetMany");
            if (request.IsSuccessStatusCode)
            {
                var serializeList = request.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<ItemModel>>(serializeList);
            }
            return null!;
        }

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

        public async Task<List<Item>> GetItemsByCategory([FromQuery]int id)
        {
            HttpResponseMessage request = await httpClient.GetAsync($"Item/FilterByCategory/{id}");
            request.EnsureSuccessStatusCode();
            var serializeList = request.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Item>>(serializeList);
        }

        public async Task<ActionResult> SearchModel(string searchModel)
        {
            HttpResponseMessage request = await httpClient.GetAsync($"ItemModel/SearchModel/{searchModel}");
            var serializeList = request.Content.ReadAsStringAsync().Result;
            List<ItemModel> list = JsonConvert.DeserializeObject<List<ItemModel>>(serializeList);
            return Json(list);
        }
        #endregion
    }
}