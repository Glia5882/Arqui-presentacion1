using Microsoft.AspNetCore.Mvc;
using ProyectoNotas.DAO;
using ProyectoNotas.Models;

namespace ProyectoNotas.Controllers
{
    public class InscripcionesWebController : Controller
    {
        private readonly IMateriaDAO _materiaDAO;
        private readonly IInscripcionDAO _inscripcionDAO;

        public InscripcionesWebController(IMateriaDAO materiaDAO, IInscripcionDAO inscripcionDAO)
        {
            _materiaDAO = materiaDAO;
            _inscripcionDAO = inscripcionDAO;
        }

        [HttpGet]
        public async Task<IActionResult> ListarMaterias()
        {
            var materias = await _materiaDAO.ObtenerTodas();
            return View(materias);
        }

        [HttpPost]
        public async Task<IActionResult> Inscribirse(int materiaId)
        {
            var estudianteId = HttpContext.Session.GetInt32("EstudianteId");
            if (!estudianteId.HasValue)
                return RedirectToAction("Login", "AccountWeb");

            var nuevaInscripcion = new Inscripcion
            {
                EstudianteId = estudianteId.Value,
                MateriaId = materiaId,
                Notas = new List<float>()
            };

            await _inscripcionDAO.Crear(nuevaInscripcion);
            return RedirectToAction("Index", "EstudiantesWeb");
        }

        [HttpPost]
        public async Task<IActionResult> AgregarNota(int inscripcionId, string nota) {
            var inscripcion = await _inscripcionDAO.BuscarPorId(inscripcionId);
            if (inscripcion == null)
                return RedirectToAction("Index", "MateriasWeb");

            // Reemplazar coma por punto y convertir
            if (!float.TryParse(nota.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float notaParseada))
            {
                Console.WriteLine("⚠️ No se pudo convertir la nota");
                return RedirectToAction("Detalles", "MateriasWeb", new { id = inscripcion.MateriaId });
            }

            Console.WriteLine($"✅ Agregando nota: {notaParseada} a la inscripción ID: {inscripcionId}");
            inscripcion.Notas.Add(notaParseada);
            await _inscripcionDAO.Actualizar(inscripcion);

            return RedirectToAction("Detalles", "MateriasWeb", new { id = inscripcion.MateriaId });
        }
    }
}
