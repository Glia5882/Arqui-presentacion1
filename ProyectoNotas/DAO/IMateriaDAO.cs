using ProyectoNotas.Models;

namespace ProyectoNotas.DAO
{
    public interface IMateriaDAO
    {
        Task<List<Materia>> ObtenerTodas();
        Task<Materia?> BuscarPorId(int id);
        Task<Materia> Crear(Materia materia);
        Task Actualizar(Materia materia);
        Task Eliminar(int id);
    }
}
