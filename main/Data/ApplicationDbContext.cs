using Microsoft.EntityFrameworkCore;
using main.Models;

public class ApplicationDbContext : DbContext
{
    // Constructor usado por el backend real: la cadena de conexión llega
    // inyectada desde Program.cs (appsettings.json).
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Constructor sin parámetros: solo lo usan herramientas de diseño
    // (ej. "dotnet ef migrations add") cuando no hay un host de DI armado.
    public ApplicationDbContext()
    {
    }

    public DbSet<Alerta> Alertas { get; set; }
    public DbSet<Barrio> Barrios { get; set; }
    public DbSet<Barrio_Alerta> AlertasBarrios { get; set; }
    public DbSet<Ciudadano> Ciudadanos { get; set; }
    public DbSet<Reporte> Reportes { get; set; }
    public DbSet<Ubicacion> Ubicaciones { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = "Server=localhost;Database=incidencias_urbanas;User=root;Password=";

            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)
            );
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Un DNI no debería repetirse entre ciudadanos.
        modelBuilder.Entity<Ciudadano>()
            .HasIndex(c => c.dni)
            .IsUnique();
    }
}
