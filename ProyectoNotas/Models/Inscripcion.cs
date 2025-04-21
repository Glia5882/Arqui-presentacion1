namespace ProyectoNotas.Models
{
    public class Inscripcion
    {
        public int Id { get; set; }

        // Relaciones
        public int EstudianteId { get; set; }
        public Estudiante? Estudiante { get; set; }

        public int MateriaId { get; set; }
        public Materia? Materia { get; set; }

        public List<float> Notas { get; set; } = new();
    }
}
