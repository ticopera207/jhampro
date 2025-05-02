using Microsoft.EntityFrameworkCore;
using Jham.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Cita> Citas { get; set; }
    public DbSet<Caso> Casos { get; set; }
    public DbSet<Documento> Documentos { get; set; }
    public DbSet<Retroalimentacion> Retroalimentaciones { get; set; }
}