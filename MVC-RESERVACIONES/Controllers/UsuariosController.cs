using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_RESERVACIONES.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

               JObject jsonResponse = JObject.Parse(data);

                JArray usersArray = (JArray)jsonResponse["result"];

                ListaUsuarios = usersArray.ToObject<List<UsuarioViewModel>>();

            }
            return View(ListaUsuarios);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UsuarioViewModel model)
        {
            try
            {
                var usuarioRequest = new
                {
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Email = model.Email,
                    Psswd = model.Psswd,
                    Rol = model.Rol,
                    Estado = string.IsNullOrEmpty(model.Estado) ? "A" : model.Estado
                };

                string data = JsonConvert.SerializeObject(usuarioRequest);

                Console.WriteLine($"JSON enviado: {data}");
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response =  _client.PostAsync(_client.BaseAddress + "/Usuarios/PostUsuario", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Usuario Creado.";
                    return RedirectToAction("Index");
                }
                else
                {
                    string error = response.Content.ReadAsStringAsync().Result;
                    TempData["errorMessage"] = $"Error al crear el usuario: {error}";
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();
        }


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

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/Usuarios/DeleteUsuario/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Usuario Eliminado.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Error al eliminar el usuario.";
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

    }
}
