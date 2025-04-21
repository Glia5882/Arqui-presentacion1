using Microsoft.EntityFrameworkCore;
using ProyectoNotas.Data;
using ProyectoNotas.Models;

namespace ProyectoNotas.DAO
{
    public class InscripcionDAO : IInscripcionDAO
    {
        private readonly AppDbContext _context;

        public InscripcionDAO(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Inscripcion>> ObtenerTodas()
        {
            return await _context.Inscripciones
                .Include(i => i.Estudiante)
                .Include(i => i.Materia)
                .ToListAsync();
        }

        public async Task<Inscripcion?> BuscarPorId(int id)
        {
            return await _context.Inscripciones
                .Include(i => i.Estudiante)
                .Include(i => i.Materia)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Inscripcion> Crear(Inscripcion inscripcion)
        {
            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync();

            // 🔁 Recalcular promedio del estudiante
            await RecalcularPromedioEstudiante(inscripcion.EstudianteId);

            // 🔁 Recalcular promedio de la materia
            await RecalcularPromedioMateria(inscripcion.MateriaId);

            return await BuscarPorId(inscripcion.Id) ?? inscripcion;
        }

        public async Task Actualizar(Inscripcion inscripcion)
        {
            _context.Entry(inscripcion).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            await RecalcularPromedioEstudiante(inscripcion.EstudianteId);
            await RecalcularPromedioMateria(inscripcion.MateriaId);
        }

        public async Task Eliminar(int id)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(id);
            if (inscripcion != null)
            {
                int estudianteId = inscripcion.EstudianteId;
                int materiaId = inscripcion.MateriaId;

                _context.Inscripciones.Remove(inscripcion);
                await _context.SaveChangesAsync();

                await RecalcularPromedioEstudiante(estudianteId);
                await RecalcularPromedioMateria(materiaId);
            }
        }

        public async Task<float> CalcularPromedioEstudiante(int estudianteId)
        {
            var inscripciones = await _context.Inscripciones
                .Where(i => i.EstudianteId == estudianteId)
                .ToListAsync();

            var notas = inscripciones.SelectMany(i => i.Notas).ToList();

            return notas.Count == 0 ? 0 : notas.Average();
        }

        private async Task RecalcularPromedioEstudiante(int estudianteId)
        {
            var estudiante = await _context.Estudiantes.FindAsync(estudianteId);
            if (estudiante != null)
            {
                estudiante.PromedioAcumulado = await CalcularPromedioEstudiante(estudianteId);
                await _context.SaveChangesAsync();
            }
        }

        private async Task RecalcularPromedioMateria(int materiaId)
        {
            var inscripciones = await _context.Inscripciones
                .Where(i => i.MateriaId == materiaId)
                .ToListAsync();

            var todasNotas = inscripciones.SelectMany(i => i.Notas).ToList();
            float promedio = todasNotas.Count == 0 ? 0 : todasNotas.Average();

            var materia = await _context.Materias.FindAsync(materiaId);
            if (materia != null)
            {
                materia.PromedioMateria = promedio;
                await _context.SaveChangesAsync();
            }
        }
    }
}
