using Microsoft.AspNetCore.Mvc;
using ProyectoNotas.Models;
using ProyectoNotas.Models.ViewModels;
using ProyectoNotas.DAO;

namespace ProyectoNotas.Controllers
{
    public class AccountWebController : Controller
    {
        private readonly IEstudianteDAO _estudianteDAO;

        public AccountWebController(IEstudianteDAO estudianteDAO)
        {
            _estudianteDAO = estudianteDAO;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var estudiante = await _estudianteDAO.BuscarPorCredenciales(model.Username, model.Password);
                if (estudiante != null)
                {   
                    HttpContext.Session.SetInt32("EstudianteId", estudiante.Id);
                    return RedirectToAction("Index", "EstudiantesWeb");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model) {

            if (ModelState.IsValid)
            {
                try
                {
                    var nuevoEstudiante = new Estudiante
                    {
                        Usuario = model.Username,
                        Contrasena = model.Password,
                        PromedioAcumulado = 0,
                        Inscripciones = new List<Inscripcion>()
                    };

                    await _estudianteDAO.Crear(nuevoEstudiante);
                    return RedirectToAction("Login");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Error al registrar el usuario. Intente nuevamente.");
                }
            }

            return View(model);
        }
    }
}
