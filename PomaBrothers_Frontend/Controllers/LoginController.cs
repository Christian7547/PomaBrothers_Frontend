using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PomaBrothers_Frontend.Models;
using System.Net.Http.Headers;
using System.Security.Claims;
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
                var requestContent = new StringContent("", Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync($"api/Login/Login?user={user}&password={password}", requestContent);

                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<Employee>(json);

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, result!.Id.ToString()),
                        new Claim(ClaimTypes.Name, $"{result.Name} {result.LastName} {result.SecondLastName}"),
                        new Claim(ClaimTypes.Role, result.Role)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    ViewData["WelcomeMessage"] = "Bienvenido " + user;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["ErrorMessage"] = "Credenciales incorrectas";
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}

