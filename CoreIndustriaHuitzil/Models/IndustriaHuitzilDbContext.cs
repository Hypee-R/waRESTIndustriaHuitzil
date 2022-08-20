using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CoreIndustriaHuitzil.Models
{
    public partial class IndustriaHuitzilDbContext : DbContext
    {
        public IndustriaHuitzilDbContext()
        {
        }

        public IndustriaHuitzilDbContext(DbContextOptions<IndustriaHuitzilDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Articulo> Articulos { get; set; } = null!;
        public virtual DbSet<CatCategoria> CatCategorias { get; set; } = null!;
        public virtual DbSet<CatProveedore> CatProveedores { get; set; } = null!;
        public virtual DbSet<CatTalla> CatTallas { get; set; } = null!;
        public virtual DbSet<CatUbicacione> CatUbicaciones { get; set; } = null!;
        public virtual DbSet<Materiale> Materiales { get; set; } = null!;
        public virtual DbSet<ProveedoresMateriale> ProveedoresMateriales { get; set; } = null!;
        public virtual DbSet<Rol> Rols { get; set; } = null!;
        public virtual DbSet<SolicitudesMateriale> SolicitudesMateriales { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Vista> Vistas { get; set; } = null!;
        public virtual DbSet<VistasRol> VistasRols { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Server=localhost;Database=IndustriaHuitzil;Trusted_Connection=false;MultipleActiveResultSets=true;User ID=sa;Password=Ventana0512");
                optionsBuilder.UseSqlServer("Server=DESKTOP-4HBFH8F\\SQLEXPRESS;Database=IndustriaHuitzil;Trusted_Connection=false;MultipleActiveResultSets=true;User ID=sa;Password=admin");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articulo>(entity =>
            {
                entity.HasKey(e => e.IdArticulo)
                    .HasName("PK_Articulos_1");

                entity.Property(e => e.IdArticulo).HasColumnName("id_articulo");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Existencia)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("existencia");

                entity.Property(e => e.FechaIngreso)
                    .HasColumnType("date")
                    .HasColumnName("fecha_ingreso");

                entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");

                entity.Property(e => e.IdTalla).HasColumnName("id_talla");

                entity.Property(e => e.IdUbicacion).HasColumnName("id_ubicacion");

                entity.Property(e => e.Unidad)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("unidad");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.Articulos)
                    .HasForeignKey(d => d.IdCategoria)
                    .HasConstraintName("FK_Articulos_Categorias");

                entity.HasOne(d => d.IdTallaNavigation)
                    .WithMany(p => p.Articulos)
                    .HasForeignKey(d => d.IdTalla)
                    .HasConstraintName("FK_Articulos_Tallas");

                entity.HasOne(d => d.IdUbicacionNavigation)
                    .WithMany(p => p.Articulos)
                    .HasForeignKey(d => d.IdUbicacion)
                    .HasConstraintName("FK_Articulos_Ubicaciones");
            });

            modelBuilder.Entity<CatCategoria>(entity =>
            {
                entity.HasKey(e => e.IdCategoria);

                entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<CatProveedore>(entity =>
            {
                entity.HasKey(e => e.IdProveedor);

                entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(50)
                    .HasColumnName("apellido_materno");

                entity.Property(e => e.ApellidoPaterno)
                    .HasMaxLength(50)
                    .HasColumnName("apellido_paterno");

                entity.Property(e => e.Correo)
                    .HasMaxLength(50)
                    .HasColumnName("correo");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(80)
                    .HasColumnName("direccion");

                entity.Property(e => e.EncargadoNombre)
                    .HasMaxLength(50)
                    .HasColumnName("encargado_nombre");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .HasColumnName("nombre");

                entity.Property(e => e.Telefono1)
                    .HasMaxLength(50)
                    .HasColumnName("telefono1");

                entity.Property(e => e.Telefono2)
                    .HasMaxLength(80)
                    .HasColumnName("telefono2");

                entity.Property(e => e.Visible)
                    .IsRequired()
                    .HasColumnName("visible")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<CatTalla>(entity =>
            {
                entity.HasKey(e => e.IdTalla);

                entity.Property(e => e.IdTalla).HasColumnName("id_talla");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Visible)
                    .IsRequired()
                    .HasColumnName("visible")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<CatUbicacione>(entity =>
            {
                entity.HasKey(e => e.IdUbicacion);

                entity.Property(e => e.IdUbicacion).HasColumnName("id_ubicacion");

                entity.Property(e => e.ApellidoMEncargado)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("apellidoM_encargado");

                entity.Property(e => e.ApellidoPEncargado)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("apellidoP_encargado");

                entity.Property(e => e.Correo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("correo");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("direccion");

                entity.Property(e => e.NombreEncargado)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre_encargado");

                entity.Property(e => e.Telefono1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("telefono1");

                entity.Property(e => e.Telefono2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("telefono2");
            });

            modelBuilder.Entity<Materiale>(entity =>
            {
                entity.HasKey(e => e.IdMaterial);

                entity.Property(e => e.IdMaterial).HasColumnName("id_material");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .HasColumnName("nombre");

                entity.Property(e => e.Precio).HasColumnName("precio");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .HasColumnName("status")
                    .IsFixedLength();

                entity.Property(e => e.Stock).HasColumnName("stock");

                entity.Property(e => e.TipoMedicion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tipo_medicion");

                entity.Property(e => e.Visible)
                    .IsRequired()
                    .HasColumnName("visible")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<ProveedoresMateriale>(entity =>
            {
                entity.HasKey(e => e.IdProveedorMaterial)
                    .HasName("PK__Proveedo__AD3DDFA9E5B7D521");

                entity.Property(e => e.IdProveedorMaterial).HasColumnName("id_proveedor_material");

                entity.Property(e => e.IdMaterial).HasColumnName("id_material");

                entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");

                entity.HasOne(d => d.IdMaterialNavigation)
                    .WithMany(p => p.ProveedoresMateriales)
                    .HasForeignKey(d => d.IdMaterial)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProveedoresMateriales_Materiales");

                entity.HasOne(d => d.IdProveedorNavigation)
                    .WithMany(p => p.ProveedoresMateriales)
                    .HasForeignKey(d => d.IdProveedor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProveedoresMateriales_CatProveedores");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol);

                entity.ToTable("Rol");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Visible)
                    .IsRequired()
                    .HasColumnName("visible")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<SolicitudesMateriale>(entity =>
            {
                entity.HasKey(e => e.IdSolicitud);

                entity.Property(e => e.IdSolicitud).HasColumnName("id_solicitud");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.Comentarios)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("comentarios");

                entity.Property(e => e.CostoTotal).HasColumnName("costo_total");

                entity.Property(e => e.Fecha)
                    .HasColumnType("date")
                    .HasColumnName("fecha");

                entity.Property(e => e.FechaUpdate)
                    .HasColumnType("date")
                    .HasColumnName("fecha_update");

                entity.Property(e => e.IdProveedorMaterial).HasColumnName("id_proveedor_material");

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .HasColumnName("status")
                    .IsFixedLength();

                entity.HasOne(d => d.IdProveedorMaterialNavigation)
                    .WithMany(p => p.SolicitudesMateriales)
                    .HasForeignKey(d => d.IdProveedorMaterial)
                    .HasConstraintName("FK_SolicitudesMateriales_ProveedoresMateriales");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.SolicitudesMateriales)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_SolicitudesMateriales_Users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("apellido_materno");

                entity.Property(e => e.ApellidoPaterno)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("apellido_paterno");

                entity.Property(e => e.Correo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("correo");

                entity.Property(e => e.ExpiredTime)
                    .HasColumnType("datetime")
                    .HasColumnName("expired_time");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("telefono");

                entity.Property(e => e.Token)
                    .HasMaxLength(800)
                    .IsUnicode(false)
                    .HasColumnName("token");

                entity.Property(e => e.UltimoAcceso)
                    .HasColumnType("datetime")
                    .HasColumnName("ultimo_acceso");

                entity.Property(e => e.Usuario)
                    .HasMaxLength(50)
                    .HasColumnName("usuario");

                entity.Property(e => e.Visible)
                    .IsRequired()
                    .HasColumnName("visible")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK_Users_Rol");
            });

            modelBuilder.Entity<Vista>(entity =>
            {
                entity.HasKey(e => e.IdVista);

                entity.Property(e => e.IdVista).HasColumnName("id_vista");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Icon)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("icon")
                    .HasDefaultValueSql("('fa-solid fa-house')");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Posicion).HasColumnName("posicion");

                entity.Property(e => e.RouterLink)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("routerLink");

                entity.Property(e => e.Visible)
                    .IsRequired()
                    .HasColumnName("visible")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<VistasRol>(entity =>
            {
                entity.HasKey(e => e.IdVistaRol);

                entity.ToTable("VistasRol");

                entity.Property(e => e.IdVistaRol).HasColumnName("id_vista_rol");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.IdVista).HasColumnName("id_vista");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.VistasRols)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VistasRol_Rol");

                entity.HasOne(d => d.IdVistaNavigation)
                    .WithMany(p => p.VistasRols)
                    .HasForeignKey(d => d.IdVista)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VistaRol_Vista");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
