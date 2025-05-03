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
    public DbSet<Abogado> Abogados { get; set; }
    public DbSet<Rel_AbogadoCaso> Rel_AbogadoCasos { get; set; }
    public DbSet<Rel_AbogadoEspecialidad> Rel_AbogadoEspecialidades { get; set; }
    public DbSet<Especialidad> Especialidades { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Relación uno a uno: Caso ↔ Cita
    modelBuilder.Entity<Caso>()
        .HasOne(c => c.Cita)
        .WithOne(cita => cita.Caso)
        .HasForeignKey<Caso>(c => c.CitaId)
        .OnDelete(DeleteBehavior.Cascade);

    // Relación UsuarioId y UsuarioId2 en Cita
    modelBuilder.Entity<Cita>()
        .HasOne(c => c.Usuario)
        .WithMany()
        .HasForeignKey(c => c.UsuarioId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Cita>()
        .HasOne(c => c.Usuario2)
        .WithMany()
        .HasForeignKey(c => c.UsuarioId2)
        .OnDelete(DeleteBehavior.Restrict);

    // Relación muchos a muchos: Abogado ↔ Caso
    modelBuilder.Entity<Rel_AbogadoCaso>()
        .HasKey(ac => new { ac.AbogadoId, ac.CasoId });

    // Relación muchos a muchos: Abogado ↔ Especialidad
    modelBuilder.Entity<Rel_AbogadoEspecialidad>()
        .HasKey(ae => new { ae.AbogadoId, ae.EspecialidadId });

    base.OnModelCreating(modelBuilder);
}
   
    
    
}