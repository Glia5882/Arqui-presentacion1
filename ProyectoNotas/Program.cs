using Microsoft.EntityFrameworkCore;
using ProyectoNotas.Data;
using ProyectoNotas.DAO;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Configura servicios
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // ✅ Permite manejar ciclos de referencia
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews(); // 👈 necesario para vistas Razor

// 👉 Registro de DAOs
builder.Services.AddScoped<IEstudianteDAO, EstudianteDAO>();
builder.Services.AddScoped<IMateriaDAO, MateriaDAO>();
builder.Services.AddScoped<IInscripcionDAO, InscripcionDAO>();

// 🔧 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// 🔧 Registro del servicio de sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true; // Solo accesible desde el servidor
    options.Cookie.IsEssential = true; // Esencial para la aplicación
});

var app = builder.Build();

// 🔧 Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// 🔧 Mapea los controladores API
app.MapControllers();

// 🧭 Ruta para controladores con vistas (MVC)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AccountWeb}/{action=Login}/{id?}"
);

// 🔧 Configuración de sesiones
app.UseSession();

app.Run();