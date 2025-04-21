using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProyectoNotas.Models;
using System.Text.Json;

namespace ProyectoNotas.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
        public DbSet<Materia> Materias => Set<Materia>();
        public DbSet<Inscripcion> Inscripciones => Set<Inscripcion>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ Conversión de List<float> a string JSON para el campo Notas
            var notasConverter = new ValueConverter<List<float>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                v => JsonSerializer.Deserialize<List<float>>(v, (JsonSerializerOptions)null!) ?? new List<float>()
            );

            modelBuilder.Entity<Inscripcion>()
                .Property(i => i.Notas)
                .HasConversion(notasConverter);
        }
    }
}
