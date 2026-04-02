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
            modelBuilder.Entity<Boleta>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(p => p.HoraEntrada).HasMaxLength(10);
                entity.Property(p => p.HoraSalida).HasMaxLength(10);
                entity.Property(p => p.NombreSupervisor).HasMaxLength(100);
                entity.Property(p => p.NombreSupervisorPlanta).HasMaxLength(100);
                entity.Property(p => p.Folio).HasMaxLength(50);

                entity.HasOne<Traslado>()
                    .WithMany()
                    .HasForeignKey(x => x.IdTraslado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Turno>()
                    .WithMany()
                    .HasForeignKey(x => x.IdTurno)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Unidad>()
                    .WithMany()
                    .HasForeignKey(x => x.IdUnidad)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Operador>()
                    .WithMany()
                    .HasForeignKey(x => x.IdOperador)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Planta>()
                    .WithMany()
                    .HasForeignKey(x => x.IdPlanta)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BoletaUsuario>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(p => p.Nomina).HasMaxLength(20);
                entity.Property(p => p.NombreUsuario).HasMaxLength(100);
                entity.Property(p => p.Direccion).HasMaxLength(200);

                entity.HasOne<Boleta>()
                    .WithMany()
                    .HasForeignKey(x => x.IdBoleta)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Marca>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(p => p.Nombre).HasMaxLength(100);
                entity.Property(p => p.Imagen).HasMaxLength(500);
            });

            modelBuilder.Entity<Operador>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(p => p.Nombre).HasMaxLength(100);
            });

            modelBuilder.Entity<Planta>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(p => p.Nombre).HasMaxLength(100);
                entity.Property(p => p.NombreContacto).HasMaxLength(100);
                entity.Property(p => p.NumeroContacto).HasMaxLength(20);
            });

            modelBuilder.Entity<Traslado>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(p => p.Tipo).HasMaxLength(50);
            });

            modelBuilder.Entity<Turno>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(p => p.Descripcion).HasMaxLength(50);
            });

            modelBuilder.Entity<Unidad>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(p => p.NumeroUnidad).HasMaxLength(50);
                entity.Property(p => p.Marca).HasMaxLength(100);
                entity.Property(p => p.Modelo).HasMaxLength(100);
            });

            modelBuilder.Entity<UsuarioSistema>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(p => p.Nombres).HasMaxLength(100);
                entity.Property(p => p.Apellidos).HasMaxLength(100);
                entity.Property(p => p.Usuario).HasMaxLength(50);
                entity.Property(p => p.Pass).HasMaxLength(255);
            });
        }
    }
}
