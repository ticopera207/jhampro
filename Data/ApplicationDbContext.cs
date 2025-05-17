using Microsoft.EntityFrameworkCore;
using jhampro.Models;




namespace jhampro.Data

public class ApplicationDbContext : DbContext
{
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Servicio> Servicios { get; set; }
    public DbSet<AbogadoServicio> AbogadoServicio { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Relación uno a muchos (Cliente a Servicio)
        modelBuilder.Entity<Servicio>()
            .HasOne(s => s.Cliente)
            .WithMany(u => u.ServiciosComoCliente)
            .HasForeignKey(s => s.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación muchos a muchos (Abogado a Servicio) - Clave compuesta
        modelBuilder.Entity<AbogadoServicio>()
            .HasKey(us => new { us.UsuarioId, us.ServicioId });

        modelBuilder.Entity<AbogadoServicio>()
            .HasOne(us => us.Usuario)
            .WithMany(u => u.AbogadoServicios)
            .HasForeignKey(us => us.UsuarioId);

        modelBuilder.Entity<AbogadoServicio>()
            .HasOne(us => us.Servicio)
            .WithMany(s => s.AbogadoServicios)
            .HasForeignKey(us => us.ServicioId);

    base.OnModelCreating(modelBuilder);
}
   
    
    
}