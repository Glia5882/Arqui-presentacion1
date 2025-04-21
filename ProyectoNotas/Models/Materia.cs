namespace ProyectoNotas.Models
{
    public class Materia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Profesor { get; set; }
        public float PromedioMateria { get; set; }

        public List<Inscripcion> Inscripciones { get; set; } = new ();
    }
}
