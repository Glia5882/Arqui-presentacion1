using ProyectoNotas.Models;

namespace ProyectoNotas.DAO
{
    public interface IEstudianteDAO
    {
        Task<List<Estudiante>> ObtenerTodos();
        Task<Estudiante?> BuscarPorId(int id);
        Task<Estudiante> Crear(Estudiante estudiante);
        Task Actualizar(Estudiante estudiante);
        Task Eliminar(int id);
        Task<Estudiante?> BuscarPorCredenciales(string usuario, string contrasena);
        Task<Estudiante?> BuscarConInscripcionesYMaterias(int id);

    }
}
