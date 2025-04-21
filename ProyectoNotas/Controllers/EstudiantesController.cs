using Microsoft.AspNetCore.Mvc;
using ProyectoNotas.Models;
using ProyectoNotas.DAO;

namespace ProyectoNotas.Controllers
{
    /// <summary>
    /// Controlador para operaciones CRUD de estudiantes.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EstudiantesController : ControllerBase
    {
        private readonly IEstudianteDAO _estudianteDAO;

        public EstudiantesController(IEstudianteDAO estudianteDAO)
        {
            _estudianteDAO = estudianteDAO;
        }

        /// <summary>
        /// Obtiene todos los estudiantes registrados.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudiante>>> GetAll()
        {
            var estudiantes = await _estudianteDAO.ObtenerTodos();
            return Ok(estudiantes);
        }

        /// <summary>
        /// Obtiene un estudiante por su ID.
        /// </summary>
        /// <param name="id">ID del estudiante.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Estudiante>> GetById(int id)
        {
            var estudiante = await _estudianteDAO.BuscarPorId(id);
            if (estudiante == null) return NotFound();
            return Ok(estudiante);
        }

        /// <summary>
        /// Crea un nuevo estudiante.
        /// </summary>
        /// <param name="estudiante">Datos del estudiante a crear.</param>
        [HttpPost]
        public async Task<ActionResult<Estudiante>> Create(Estudiante estudiante)
        {
            var creado = await _estudianteDAO.Crear(estudiante);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        /// <summary>
        /// Actualiza un estudiante existente.
        /// </summary>
        /// <param name="id">ID del estudiante.</param>
        /// <param name="estudiante">Datos actualizados del estudiante.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Estudiante estudiante)
        {
            if (id != estudiante.Id) return BadRequest();
            await _estudianteDAO.Actualizar(estudiante);
            return NoContent();
        }

        /// <summary>
        /// Elimina un estudiante por su ID.
        /// </summary>
        /// <param name="id">ID del estudiante.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _estudianteDAO.Eliminar(id);
            return NoContent();
        }
    }
}
