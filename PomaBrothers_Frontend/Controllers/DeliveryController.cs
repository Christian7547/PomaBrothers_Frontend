using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            HttpResponseMessage request = await httpClient.GetAsync("Delivery/GetMany");
            if (request.IsSuccessStatusCode)
            {
                var json = request.Content.ReadAsStringAsync().Result;
                var getItem = JsonConvert.DeserializeObject<List<Invoice>>(json);
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
        public IActionResult MiAccion(string nombre, string edad)
        {
            // Procesar el objeto JSON recibido desde la solicitud AJAX
            //string nombre = datos["nombre"].ToString();
            //int edad = datos["edad"].Value<int>();

            // Realizar alguna lógica

            return Json(new { mensajse = "Datos recibidos y procesados correctamente." });
        }


        [HttpPost]
        public async Task<IActionResult> New([FromBody] JObject json)
        {
            /*try
            {*/

            StreamReader reader = new StreamReader(Request.Body);

            string requestBody = await reader.ReadToEndAsync();
            Console.WriteLine(json);
            DeliveryDTO objDelivery = DeliveryDTO.FromJson(json);
            HttpResponseMessage request = await httpClient.PostAsJsonAsync("Delivery/New", objDelivery);
            if (request.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Delivery");
            }
            return View();
            /*}
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }*/
        }
        //[HttpPost]
        //public async Task<IActionResult> Create(Item item, IFormFile image)
        //{
        //    try
        //    {
        //        Stream img = image.OpenReadStream();
        //        string url = await StorageItem(img, image.FileName);
        //        item.UrlImage = url;
        //        HttpResponseMessage request = await httpClient.PostAsJsonAsync("Item/New", item);
        //        request.EnsureSuccessStatusCode();
        //        return RedirectToAction("Index", "Item");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

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
