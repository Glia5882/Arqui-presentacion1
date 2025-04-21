namespace ProyectoNotas.Models;
using System.ComponentModel.DataAnnotations;

public class Estudiante
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El usuario es obligatorio.")]
    [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
    public string Usuario { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(6, ErrorMessage = "Debe tener al menos 6 caracteres.")]
    public string Contrasena { get; set; }

    public float PromedioAcumulado { get; set; }

    public List<Inscripcion> Inscripciones { get; set; } = new();
}
