using Microsoft.EntityFrameworkCore;
using main.Models;

public class ApplicationDbContext : DbContext
{
    //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    //{
    //}

    // This property represents the Products table in MySQL
    public DbSet<Alerta> Alertas { get; set; }
    public DbSet<Barrio> Barrios { get; set; }
    public DbSet<Barrio_Alerta> AlertasBarrios { get; set; }
    public DbSet<Ciudadano> Ciudadanos { get; set; }
    public DbSet<Reporte> Reportes { get; set; }
    public DbSet <Ubicacion> Ubicaciones { get; set; }
    public DbSet <Usuario> Usuarios { get; set; }


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

}
