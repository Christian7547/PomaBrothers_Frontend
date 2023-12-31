﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PomaBrothers_Frontend.Models;
using System.Net.Http.Headers;
using System.Text;

namespace PomaBrothers_Frontend.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            ViewData["alert"] = "hidden";
            return View();
        }

        private HttpClient httpClient = new();

        public LoginController()
        {
            httpClient.BaseAddress = new Uri("http://localhost:5164/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpPost]
        public async Task<IActionResult> Login(string user, string password)
        {
            try
            {
                var data = new { User = user, Password = password };
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                HttpResponseMessage request = await httpClient.PutAsync($"api/Login/Login" , content);
                //request.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Employee");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
