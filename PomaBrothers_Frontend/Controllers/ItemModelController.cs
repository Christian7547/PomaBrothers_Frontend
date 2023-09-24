using Microsoft.AspNetCore.Mvc;
using PomaBrothers_Frontend.Models;
using System.Net.Http.Headers;

namespace PomaBrothers_Frontend.Controllers
{
    public class ItemModelController : Controller
    {
        private HttpClient httpClient = new();

        public ItemModelController()
        {
            httpClient.BaseAddress = new Uri("http://localhost:5164/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ItemModel itemModel)
        {
            try
            {
                HttpResponseMessage request = await httpClient.PostAsJsonAsync("ItemModel/New", itemModel);
                request.EnsureSuccessStatusCode();
                return Ok();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
