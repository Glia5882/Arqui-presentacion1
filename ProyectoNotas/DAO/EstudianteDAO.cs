using Microsoft.EntityFrameworkCore;
using ProyectoNotas.Data;
using ProyectoNotas.Models;

namespace ProyectoNotas.DAO
{
    public class EstudianteDAO : IEstudianteDAO
    {
        private readonly AppDbContext _context;

        public EstudianteDAO(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Estudiante>> ObtenerTodos()
        {
            return await _context.Estudiantes.ToListAsync();
        }

        public async Task<Estudiante?> BuscarPorId(int id)
        {
            return await _context.Estudiantes.FindAsync(id);
        }

        public async Task<Estudiante> Crear(Estudiante estudiante)
        {
            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();
            return estudiante;
        }

        public async Task Actualizar(Estudiante estudiante)
        {
            _context.Entry(estudiante).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante != null)
            {
                _context.Estudiantes.Remove(estudiante);
                await _context.SaveChangesAsync();
            }
        }
    }
}
