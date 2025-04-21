using Microsoft.EntityFrameworkCore;
using ProyectoNotas.Data;
using ProyectoNotas.Models;

namespace ProyectoNotas.DAO
{
    public class MateriaDAO : IMateriaDAO
    {
        private readonly AppDbContext _context;

        public MateriaDAO(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Materia>> ObtenerTodas()
        {
            return await _context.Materias.ToListAsync();
        }

        public async Task<Materia?> BuscarPorId(int id)
        {
            return await _context.Materias.FindAsync(id);
        }

        public async Task<Materia> Crear(Materia materia)
        {
            _context.Materias.Add(materia);
            await _context.SaveChangesAsync();
            return materia;
        }

        public async Task Actualizar(Materia materia)
        {
            _context.Entry(materia).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var materia = await _context.Materias.FindAsync(id);
            if (materia != null)
            {
                _context.Materias.Remove(materia);
                await _context.SaveChangesAsync();
            }
        }
    }
}
