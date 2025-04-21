using ProyectoNotas.Models;

namespace ProyectoNotas.DAO
{
    public interface IInscripcionDAO
    {
        Task<List<Inscripcion>> ObtenerTodas();
        Task<Inscripcion?> BuscarPorId(int id);
        Task<Inscripcion> Crear(Inscripcion inscripcion);
        Task Actualizar(Inscripcion inscripcion);
        Task Eliminar(int id);
        Task<float> CalcularPromedioEstudiante(int estudianteId);
    }
}
