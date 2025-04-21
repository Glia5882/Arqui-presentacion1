using Microsoft.AspNetCore.Mvc;
using ProyectoNotas.Models;
using ProyectoNotas.DAO;

namespace ProyectoNotas.Controllers
{
    /// <summary>
    /// Controlador para operaciones de inscripción y cálculo de promedios.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class InscripcionesController : ControllerBase
    {
        private readonly IInscripcionDAO _inscripcionDAO;

        public InscripcionesController(IInscripcionDAO inscripcionDAO)
        {
            _inscripcionDAO = inscripcionDAO;
        }

        /// <summary>
        /// Obtiene todas las inscripciones.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inscripcion>>> GetAll()
        {
            var lista = await _inscripcionDAO.ObtenerTodas();
            return Ok(lista);
        }

        /// <summary>
        /// Obtiene una inscripción por su ID.
        /// </summary>
        /// <param name="id">ID de la inscripción.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Inscripcion>> GetById(int id)
        {
            var inscripcion = await _inscripcionDAO.BuscarPorId(id);
            if (inscripcion == null) return NotFound();
            return Ok(inscripcion);
        }

        /// <summary>
        /// Crea una nueva inscripción.
        /// </summary>
        /// <param name="inscripcion">Datos de la inscripción.</param>
        [HttpPost]
        public async Task<ActionResult<Inscripcion>> Create(Inscripcion inscripcion)
        {
            var creada = await _inscripcionDAO.Crear(inscripcion);
            return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
        }

        /// <summary>
        /// Actualiza una inscripción existente.
        /// </summary>
        /// <param name="id">ID de la inscripción.</param>
        /// <param name="inscripcion">Datos actualizados.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Inscripcion inscripcion)
        {
            if (id != inscripcion.Id) return BadRequest();
            await _inscripcionDAO.Actualizar(inscripcion);
            return NoContent();
        }

        /// <summary>
        /// Elimina una inscripción por su ID.
        /// </summary>
        /// <param name="id">ID de la inscripción.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _inscripcionDAO.Eliminar(id);
            return NoContent();
        }

        /// <summary>
        /// Calcula el promedio general de notas de un estudiante.
        /// </summary>
        /// <param name="estudianteId">ID del estudiante.</param>
        /// <returns>Promedio acumulado de todas sus inscripciones.</returns>
        [HttpGet("promedio/estudiante/{estudianteId}")]
        public async Task<ActionResult<float>> CalcularPromedioEstudiante(int estudianteId)
        {
            var promedio = await _inscripcionDAO.CalcularPromedioEstudiante(estudianteId);
            return Ok(promedio);
        }
    }
}
