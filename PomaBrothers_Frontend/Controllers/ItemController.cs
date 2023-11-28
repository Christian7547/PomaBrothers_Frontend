﻿using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PomaBrothers_Frontend.Models;
using PomaBrothers_Frontend.Models.DTOModels;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Net.Http;
using SkiaSharp;

namespace PomaBrothers_Frontend.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private HttpClient httpClient = new();

        public ItemController()
        {
            httpClient.BaseAddress = new Uri("http://localhost:5164/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<string> StorageItem(Stream file, string name)
        {
            string email = "catier@gmail.com";
            string clave = "123456";
            string ruta = "pomabrothers-4c702.appspot.com";
            string api_key = "AIzaSyA6PKJivkb3Ir8zsbL21HMwCsmRTJ-GscM";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var a = await auth.SignInWithEmailAndPasswordAsync(email, clave);
            var cancel = new CancellationTokenSource();
            var task = new FirebaseStorage(
                ruta,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                }).Child("Item").Child(name).PutAsync(file, cancel.Token);
            var downloadURL = await task;
            return downloadURL;
        }


        public async Task<ActionResult> Index(int page = 1, int pageSize = 6)
        {
            var getItems = await GetItemsAsync();
            var getModels = await GetModelsAsync();
            if(getItems != null && getModels != null)
            {
                var query = getItems.Join(getModels, i => i.ModelId, m => m.Id,
                (i, m) => new
                {
                    _Name = i.Name,
                    _Price = i.Price,
                    _Color = i.Color,
                    _Serie = i.Serie,
                    _Marker = m.Marker,
                    _UrlImage = i.UrlImage,
                    ItemID = i.Id,
                    ModelID = m.Id
                }).ToList();
                int totalItems = query.Count;
                var paginatedData = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.Data = paginatedData;
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                return View();
            }
            return View();
        }

        //ventanaEmergente
        public async Task<List<Sale>> GetLastSale()
        {
            HttpResponseMessage request = await httpClient.GetAsync("Sale/GetLastSale");
            request.EnsureSuccessStatusCode();
            string json = request.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Sale>>(json);
        }
        
        //Sale
        public async Task<List<Customer>> GetCustomersAsync()
        {
            HttpResponseMessage request = await httpClient.GetAsync("Customer/GetMany");
            request.EnsureSuccessStatusCode();
            string json = request.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Customer>>(json);
        }

        public async Task<ActionResult> IndexSale()
        {

            var getItems = await GetItemsAsync();
            var getModels = await GetModelsAsync();
            if(getItems != null)
            {
                var query = getItems.Join(getModels, i => i.ModelId, m => m.Id,
                (i, m) => new
                {
                    _Id = i.Id,
                    _Name = i.Name,
                    _Price = i.Price,
                    _Color = i.Color,
                    _Serie = i.Serie,
                    _Marker = m.Marker,

                    _UrlImage = i.UrlImage,
                    ItemID = i.Id,

                    ModelID = m.Id
                }).ToList();
                ViewBag.Data = query;
            }
            var customers = await GetCustomersAsync();
            return View();
        }

        public async Task<IActionResult> PostSale(SaleDetailDTO detail)
        {
            HttpResponseMessage request = await httpClient.PostAsJsonAsync("Sale/Create", detail);
            request.EnsureSuccessStatusCode();
            return Ok();
        }

        public async Task<ActionResult> CreateSale(int customerId, decimal total, [FromBody]List<ProductSaledDTO> itemsSaled)
        {
            SaleDetailDTO detail = new SaleDetailDTO();
            List<SaleDetail> details = new();
            foreach (var item in itemsSaled)
            {
                var productSaleDTO = new ProductSaledDTO();
                productSaleDTO.ProductId = item.ProductId;
                productSaleDTO.ProductPrice = item.ProductPrice;
                productSaleDTO.ModelId = item.ModelId;
                productSaleDTO.Status = 2;
                detail.ProductSaled.Add(productSaleDTO);
            }
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            detail.Total = total;
            detail.CustomerId = customerId;
            detail.EmployeeId = int.Parse(userIdClaim);
            detail.RegisterDate = DateTime.Now;
            
            try
            {
                await PostSale(detail);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
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

        public async Task<ActionResult> Edit(int id)
        {
            
            Item item = await GetItemAsync(id);
            ViewBag.Type = item.TypeWarranty;
            ViewBag.Data = await GetCategoriesAsync();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Item item, IFormFile? image)
        {
            try
            {
                item.Status = 1;
                if (image != null)
                {
                    Stream img = image.OpenReadStream();
                    string url = await StorageItem(img, image.FileName);
                    item.UrlImage = url;
                }

                HttpResponseMessage request = await httpClient.PutAsJsonAsync("Item/Edit", item);
                request.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Item");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

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

        #region Get Models
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

        public async Task<ActionResult> GetModel(int id)
        {
            HttpResponseMessage request = await httpClient.GetAsync($"ItemModel/GetOne/{id}");
            request.EnsureSuccessStatusCode();
            var serializeModel = request.Content.ReadAsStringAsync().Result;
            var getModel = JsonConvert.DeserializeObject<ItemModel>(serializeModel);
            return Json(getModel);
        }

        #endregion

        #region Filters & Browser
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

        public async Task<ActionResult> SearchModel(string searchModel)
        {
            HttpResponseMessage request = await httpClient.GetAsync($"ItemModel/SearchModel/{searchModel}");
            var serializeList = request.Content.ReadAsStringAsync().Result;
            List<ItemModel> list = JsonConvert.DeserializeObject<List<ItemModel>>(serializeList);
            return Json(list);
        }

        public async Task<ActionResult> SearchProduct(string searchProduct)
        {
            HttpResponseMessage request = await httpClient.GetAsync($"Item/SearchProduct/{searchProduct}");
            var serializeList = request.Content.ReadAsStringAsync().Result;
            List<ProductSearchDTO> list = JsonConvert.DeserializeObject<List<ProductSearchDTO>>(serializeList);
            return Json(list);
        }
        #endregion
    }
}