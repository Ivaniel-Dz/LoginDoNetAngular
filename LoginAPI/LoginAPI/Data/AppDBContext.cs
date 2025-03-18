using LoginAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginAPI.Data
{
    public partial class AppDBContext : DbContext
    {
        // Constructor por defecto
        public AppDBContext() { }

        // Constructor que recibe opciones de configuración del contexto
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        // Definir las tablas del contexto
        public virtual DbSet<Producto> Productos { get; set; } // Tabla Producto
        public virtual DbSet<Usuario> Usuarios { get; set; } // Tabla Usuario

        // Método para configurar la base de datos (se deja vacío para evitar sobreescritura)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        // Configuración del modelo y mapeo de las entidades a las tablas
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la tabla Producto
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Producto__3214EC07E8909AFD"); // Clave primaria de la tabla Producto

                entity.ToTable("Producto"); // Nombre de la tabla

                // Configuración de las propiedades de la entidad Producto
                entity.Property(e => e.Marca)
                    .HasMaxLength(50) // Longitud máxima de 50 caracteres
                    .IsUnicode(false); // No usa Unicode
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)"); // Tipo decimal con 2 decimales
            });

            // Configuración de la tabla Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC07F27BC158"); // Clave primaria de la tabla Usuario

                entity.ToTable("Usuario"); // Nombre de la tabla

                // Configuración de las propiedades de la entidad Usuario
                entity.Property(e => e.Clave)
                    .HasMaxLength(100) // Longitud máxima de 100 caracteres
                    .IsUnicode(false); // No usa Unicode
                entity.Property(e => e.Correo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCrOneatingPartial(modelBuilder); // Método parcial para extensiones adicionales de configuración
        }

        // Método parcial que puede ser sobrescrito en otras partes del código
        partial void OnModelCrOneatingPartial(ModelBuilder modelBuilder);
    }
}
