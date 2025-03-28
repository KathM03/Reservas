using System.Text;
using Microsoft.AspNetCore.Mvc;
using MVC_RESERVACIONES.Models;
using Newtonsoft.Json;

namespace MVC_RESERVACIONES.Controllers
{
    public class LoginController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44316/api");
        private readonly HttpClient _client;

        public LoginController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "Login/Login/login", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseData = response.Content.ReadAsStringAsync().Result;
                    var tokenResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseData);

                    if (tokenResponse != null && tokenResponse.ContainsKey("token"))
                    {
                        HttpContext.Session.SetString("AuthToken", tokenResponse["token"]);
                        TempData["successMessage"] = "Inicio de Sesión Exitoso";

                        return RedirectToAction("Index", "Usuarios");
                    }
                }
                TempData["errorMessage"] = "Credenciales incorrectas.";
                return View();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
    }
}
