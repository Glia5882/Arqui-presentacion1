using Microsoft.AspNetCore.Mvc;
using ProyectoNotas.DAO;
using ProyectoNotas.Models;
using Microsoft.AspNetCore.Http;

namespace ProyectoNotas.Controllers
{
    public class EstudiantesWebController : Controller
    {
        private readonly IEstudianteDAO _estudianteDAO;

        public EstudiantesWebController(IEstudianteDAO estudianteDAO)
        {
            _estudianteDAO = estudianteDAO;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var estudianteId = HttpContext.Session.GetInt32("EstudianteId");

            if (!estudianteId.HasValue)
            {
                // Si no hay sesión activa, redirigir al login
                return RedirectToAction("Login", "AccountWeb");
            }

            var estudiante = await _estudianteDAO.BuscarConInscripcionesYMaterias(estudianteId.Value);

            if (estudiante == null)
            {
                // En caso de que el ID no exista en base de datos
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "AccountWeb");
            }

            return View(estudiante);
        }

        [HttpGet]
        public async Task<IActionResult> EditarPerfil() {
            var estudianteId = HttpContext.Session.GetInt32("EstudianteId");
            if (!estudianteId.HasValue)
                return RedirectToAction("Login", "AccountWeb");

            var estudiante = await _estudianteDAO.BuscarPorId(estudianteId.Value);
            if (estudiante == null)
                return RedirectToAction("Login", "AccountWeb");

            return View(estudiante);
        }

        [HttpPost]
        public async Task<IActionResult> EditarPerfil(Estudiante actualizado)
        {
            if (!ModelState.IsValid)
                return View(actualizado);

            var estudianteId = HttpContext.Session.GetInt32("EstudianteId");
            if (!estudianteId.HasValue || estudianteId != actualizado.Id)
                return RedirectToAction("Login", "AccountWeb");

            await _estudianteDAO.Actualizar(actualizado);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EliminarCuenta()
        {
            var estudianteId = HttpContext.Session.GetInt32("EstudianteId");
            if (!estudianteId.HasValue)
                return RedirectToAction("Login", "AccountWeb");

            await _estudianteDAO.Eliminar(estudianteId.Value);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "AccountWeb");
        }
    }
}
