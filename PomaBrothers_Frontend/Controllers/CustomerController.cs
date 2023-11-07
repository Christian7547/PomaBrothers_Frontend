using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PomaBrothers_Frontend.Models.DTOModels;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace PomaBrothers_Frontend.Controllers
{
    public class CustomerController : Controller
    {
        private HttpClient httpClient = new();

        public CustomerController()
        {
            httpClient.BaseAddress = new Uri("http://localhost:5164/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ActionResult> SearchCustomer(string searchCustomer)
        {
            HttpResponseMessage request = await httpClient.GetAsync($"Customer/SearchCustomer/{searchCustomer}");
            var serializeList = request.Content.ReadAsStringAsync().Result;
            List<CustomerDTO> list = JsonConvert.DeserializeObject<List<CustomerDTO>>(serializeList);
            return Json(list);
        }
    }
}
