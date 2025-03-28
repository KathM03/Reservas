using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_RESERVACIONES.Models;
using Newtonsoft.Json;
using System.Text;

namespace MVC_RESERVACIONES.Controllers
{
    public class UsuariosController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44316/api");
        private readonly HttpClient _client;

        public UsuariosController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<UsuarioViewModel> ListaUsuarios = new List<UsuarioViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Usuarios/GetUsuarios").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;

                ListaUsuarios = JsonConvert.DeserializeObject<List<UsuarioViewModel>>(data);
            }
            return View(ListaUsuarios);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Create(UsuarioViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Usuarios/PostUsuario", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Usuario Creado.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();
        }

        [Authorize(Roles = "Administrador")]

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                UsuarioViewModel usuario = new UsuarioViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Usuarios/GetUsuario/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    usuario = JsonConvert.DeserializeObject<UsuarioViewModel>(data);
                }
                return View(usuario);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }


        [Authorize(Roles = "Administrador")]

        [HttpPost]
        public IActionResult Edit(UsuarioViewModel model)
        {

            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Usuarios/PutUsuario", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Usuario Actualizado.";
                    return RedirectToAction("Index");
                }
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
