using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// --- Servicios ---

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Evita ciclos infinitos al serializar (Reporte -> Ubicacion -> Reporte, etc.)
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default")
        ?? "Server=localhost;Database=incidencias_urbanas;User=root;Password=";

    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// CORS abierto para desarrollo. Si el front se sirve desde este mismo backend
// (que es lo que hace este Program.cs) no hace falta, pero lo dejamos por si
// en algún momento se sirve el front por separado (ej: Live Server de VSCode).
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

// --- Middlewares ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

// Sirve el frontend estático (main/front) como sitio por defecto.
// Con esto, al levantar el backend y entrar a http://localhost:PUERTO/
// se abre directamente login.html / index.html, y script.js puede pedirle
// datos a la API con rutas relativas como fetch('/api/reportes').
var frontPath = Path.Combine(builder.Environment.ContentRootPath, "front");
var frontFileProvider = new PhysicalFileProvider(frontPath);

app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = frontFileProvider,
    DefaultFileNames = new List<string> { "login.html" }
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = frontFileProvider
});

app.MapControllers();

app.Run();
