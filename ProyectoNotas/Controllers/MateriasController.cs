using Microsoft.AspNetCore.Mvc;
using ProyectoNotas.Models;
using ProyectoNotas.DAO;

namespace ProyectoNotas.Controllers
{
    /// <summary>
    /// Controlador para operaciones CRUD de materias.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MateriasController : ControllerBase
    {
        private readonly IMateriaDAO _materiaDAO;

        public MateriasController(IMateriaDAO materiaDAO)
        {
            _materiaDAO = materiaDAO;
        }

        /// <summary>
        /// Obtiene todas las materias.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Materia>>> GetAll()
        {
            var materias = await _materiaDAO.ObtenerTodas();
            return Ok(materias);
        }

        /// <summary>
        /// Obtiene una materia por su ID.
        /// </summary>
        /// <param name="id">ID de la materia.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Materia>> GetById(int id)
        {
            var materia = await _materiaDAO.BuscarPorId(id);
            if (materia == null) return NotFound();
            return Ok(materia);
        }

        /// <summary>
        /// Crea una nueva materia.
        /// </summary>
        /// <param name="materia">Datos de la materia a crear.</param>
        [HttpPost]
        public async Task<ActionResult<Materia>> Create(Materia materia)
        {
            var creada = await _materiaDAO.Crear(materia);
            return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
        }

        /// <summary>
        /// Actualiza una materia existente.
        /// </summary>
        /// <param name="id">ID de la materia.</param>
        /// <param name="materia">Datos actualizados de la materia.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Materia materia)
        {
            if (id != materia.Id) return BadRequest();
            await _materiaDAO.Actualizar(materia);
            return NoContent();
        }

        /// <summary>
        /// Elimina una materia por su ID.
        /// </summary>
        /// <param name="id">ID de la materia.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _materiaDAO.Eliminar(id);
            return NoContent();
        }
    }
}
