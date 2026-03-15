using Boletas.Models;
using Microsoft.EntityFrameworkCore;

namespace Boletas.Data
{
    public class BoletasDbContext : DbContext
    {
        public BoletasDbContext(DbContextOptions<BoletasDbContext> options) : base(options){}
        
        public DbSet<Boleta> Boletas { get; set; }
        public DbSet<BoletaUsuario> BoletaUsuarios { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Operador> Operadores { get; set; }
        public DbSet<Planta> Plantas { get; set; }
        public DbSet<Traslado> Traslados { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Unidad> Unidades { get; set; }
        public DbSet<UsuarioSistema> UsuariosSistema { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Boleta>(e => e.HasKey(k => k.Id));
            modelBuilder.Entity<BoletaUsuario>(e => e.HasKey(k => k.Id));
            modelBuilder.Entity<Marca>(e => e.HasKey(k => k.Id));
            modelBuilder.Entity<Operador>(e => e.HasKey(k => k.Id));
            modelBuilder.Entity<Planta>(e => e.HasKey(k => k.Id));
            modelBuilder.Entity<Traslado>(e => e.HasKey(k => k.Id));
            modelBuilder.Entity<Turno>(e => e.HasKey(k => k.Id));
            modelBuilder.Entity<Unidad>(e => e.HasKey(k => k.Id));
            modelBuilder.Entity<UsuarioSistema>(e => e.HasKey(k => k.Id));
        }
    }
}
