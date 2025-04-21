using Microsoft.AspNetCore.Mvc;
using ProyectoNotas.DAO;
using ProyectoNotas.Models;

namespace ProyectoNotas.Controllers
{
    public class MateriasWebController : Controller
    {
        private readonly IMateriaDAO _materiaDAO;

        public MateriasWebController(IMateriaDAO materiaDAO) {
            _materiaDAO = materiaDAO;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            var materias = await _materiaDAO.ObtenerTodas();
            return View(materias);
        }

        [HttpGet]
        public IActionResult Agregar() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(Materia model) {
            if (ModelState.IsValid)
            {
                try
                {
                    var nuevaMateria = new Materia
                    {
                        Nombre = model.Nombre,
                        Profesor = model.Profesor,
                        PromedioMateria = 0,
                        Inscripciones = new List<Inscripcion>()
                    };

                    Console.WriteLine($"Agregando materia: {nuevaMateria.Nombre} {nuevaMateria.Profesor}");

                    await _materiaDAO.Crear(nuevaMateria);
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Error al agregar la materia. Intente nuevamente.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id) {
            var materia = await _materiaDAO.BuscarPorId(id);
            if (materia == null)
            {
                return RedirectToAction("Index");
            }

            return View(materia);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Materia actualizada) {
            if (!ModelState.IsValid)
                return View(actualizada);

            await _materiaDAO.Actualizar(actualizada);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id) {  
            var materia = await _materiaDAO.BuscarPorId(id);
            if (materia == null) {
                return RedirectToAction("Index");
            } 
            await _materiaDAO.Eliminar(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var materia = await _materiaDAO.BuscarConInscripcionesYEstudiantes(id);
            if (materia == null)
                return RedirectToAction("Index");

            return View(materia);
        }
    }
}
