using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ApiVuelos.Models
{
    public partial class DataVuelosContext : DbContext
    {
        public DataVuelosContext()
        {
        }

        public DataVuelosContext(DbContextOptions<DataVuelosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Compra> Compras { get; set; } = null!;
        public virtual DbSet<DetalleVuelo> DetalleVuelos { get; set; } = null!;
        public virtual DbSet<Vuelo> Vuelos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("CadenaSql"));

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Compra>(entity =>
            {
                entity.Property(e => e.Asientos)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CostoTotal).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<DetalleVuelo>(entity =>
            {
                entity.ToTable("DetalleVuelo");

                entity.Property(e => e.Asiento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrecioAsiento).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdCompraNavigation)
                    .WithMany(p => p.DetalleVuelos)
                    .HasForeignKey(d => d.IdCompra)
                    .HasConstraintName("FK_DetalleVuelo_IdCompra");

                entity.HasOne(d => d.IdVueloNavigation)
                    .WithMany(p => p.DetalleVuelos)
                    .HasForeignKey(d => d.IdVuelo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetalleVuelo_IdVuelo");
            });

            modelBuilder.Entity<Vuelo>(entity =>
            {
                entity.Property(e => e.Aerolinea)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Destino)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaLlegada).HasColumnType("smalldatetime");

                entity.Property(e => e.FechaSalida).HasColumnType("smalldatetime");

                entity.Property(e => e.Origen)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PrecioVuelo).HasColumnType("decimal(18, 2)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
