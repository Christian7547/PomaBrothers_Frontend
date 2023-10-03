using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PomaBrothers_Frontend.Models;
using System.Net.Http.Headers;

namespace PomaBrothers_Frontend.Controllers
{
    public class EmployeeController : Controller
    {
        private HttpClient httpClient = new();

        public EmployeeController()
        {
            httpClient.BaseAddress = new Uri("http://localhost:5164/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ActionResult> Index()
        {
            var employees = await GetEmployeesAsync();
            ViewBag.Data = employees;
            return View();
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            HttpResponseMessage request = await httpClient.GetAsync("api/Employee");
            if (request.IsSuccessStatusCode)
            {
                var serializeList = await request.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Employee>>(serializeList);
            }
            return new List<Employee>();
        }

        public async Task<ActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            try
            {
                HttpResponseMessage request = await httpClient.PostAsJsonAsync("api/Employee", employee);
                request.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Employee");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            Employee employee = await GetEmployeeAsync(id);
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            try
            {
                HttpResponseMessage request = await httpClient.PutAsJsonAsync($"api/Employee/{employee.Id}", employee);
                request.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Employee");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Employee> GetEmployeeAsync(int id)
        {
            HttpResponseMessage request = await httpClient.GetAsync($"api/Employee/{id}");
            if (request.IsSuccessStatusCode)
            {
                var serializeObject = await request.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Employee>(serializeObject);
            }
            return null!;
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> GetEmployee([FromQuery] int id)
        {
            var employee = await GetEmployeeAsync(id);
            return Ok(employee);
        }

        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage request = await httpClient.DeleteAsync($"api/Employee/{id}");
            request.EnsureSuccessStatusCode();
            return RedirectToAction("Index", "Employee");
        }


        //// GET: Employee/Login
        //public IActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(string user, string password)
        //{
        //    // Validar las credenciales (aquí puedes verificar en tu base de datos)
        //    //var employee = await _context.Employees.FirstOrDefaultAsync(e => e.User == user && e.Password == password);

        //    //if (employee != null)
        //    //{
        //    //    // Autenticación exitosa
        //    //    // Puedes agregar lógica adicional aquí

        //    //    // Redireccionar a la página de inicio después del inicio de sesión
        //    //    return RedirectToAction("Index", "Home");
        //    //}
        //    //else
        //    //{
        //    //    // Autenticación fallida
        //    //    ViewBag.ErrorMessage = "Credenciales incorrectas";
        //    //    return View();
        //    //}
        //}
    }
}