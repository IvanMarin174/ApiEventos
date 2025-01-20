using Microsoft.EntityFrameworkCore;
using WebAPIEventos.Models;

namespace WebAPIEventos.Context
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Rol> Rols { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Evento> Events { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración One-to-Many
            modelBuilder.Entity<Rol>()
                .HasMany(b => b.Usuario)
                .WithOne(p => p.Rol)
                .HasForeignKey(p => p.RolId);

            // Configuración One-to-Many
            modelBuilder.Entity<Usuario>()
                .HasMany(b => b.Eventos)        
                .WithOne(p => p.Usuario)         
                .HasForeignKey(p => p.UsuarioId);
            //seed rol
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "admin"},
                new Rol {Id = 2, Nombre = "user"}
            );
            modelBuilder.Entity<Usuario>().HasData(
                    new Usuario {Id = 1, Nombre= "ivan", Correo= "ivan@correo.com", Password = "12345", RolId = 2}
                );

           
        }
        

    }
}
