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
        public virtual DbSet<Apartados> Apartados { get; set; } = null!;
        public virtual DbSet<Caja> Cajas { get; set; } = null!;
        public virtual DbSet<CambiosDevolucione> CambiosDevoluciones { get; set; } = null!;
        public virtual DbSet<CambiosDevolucionesArticulo> CambiosDevolucionesArticulos { get; set; } = null!;
        public virtual DbSet<CatCategoria> CatCategorias { get; set; } = null!;
        public virtual DbSet<CatCliente> CatClientes { get; set; } = null!;
        public virtual DbSet<CatProveedore> CatProveedores { get; set; } = null!;
        public virtual DbSet<CatTalla> CatTallas { get; set; } = null!;
        public virtual DbSet<CatUbicacione> CatUbicaciones { get; set; } = null!;
        public virtual DbSet<Materiale> Materiales { get; set; } = null!;
        public virtual DbSet<MaterialesUbicacione> MaterialesUbicaciones { get; set; } = null!;
        public virtual DbSet<ProveedoresMateriale> ProveedoresMateriales { get; set; } = null!;
        public virtual DbSet<PagoApartado> PagoApartados { get; set; } = null!;
        public virtual DbSet<Rol> Rols { get; set; } = null!;
        public virtual DbSet<SolicitudesMateriale> SolicitudesMateriales { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Venta> Ventas { get; set; } = null!;
        public virtual DbSet<VentaArticulo> VentaArticulos { get; set; } = null!;
        public virtual DbSet<Vista> Vistas { get; set; } = null!;
        public virtual DbSet<VistasRol> VistasRols { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:huitzildesarrollo.database.windows.net,1433;Initial Catalog=IndustriaHuitzil;Persist Security Info=False;User ID=HuitzilDEV;Password=Ventana0512;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Apartados>(entity =>
            {
                entity.HasKey(e => e.IdApartado);

                entity.Property(e => e.IdApartado).HasColumnName("id_Apartado");

                entity.Property(e => e.IdEmpleado)
                    .HasMaxLength(50)
                    .HasColumnName("id_empleado")
                    .IsFixedLength();

                entity.Property(e => e.idArticulo)
                    .HasMaxLength(50)
                    .HasColumnName("id_articulo")
                    .IsFixedLength();

                entity.Property(e => e.Direccion)
                    .HasMaxLength(50)
                    .HasColumnName("direccion")
                    .IsFixedLength();

                entity.Property(e => e.IdTalla)
                    .HasMaxLength(50)
                    .HasColumnName("idTalla")
                    .IsFixedLength();

                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .HasColumnName("telefono")
                    .IsFixedLength();

                entity.Property(e => e.Fecha)
                    .HasMaxLength(50)
                    .HasColumnName("fecha")
                    .IsFixedLength();

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status")
                    .IsFixedLength();

                entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type")
                .IsFixedLength ();

                entity.Property(e => e.FechaEntrega)
                    .HasMaxLength(50)
                    .HasColumnName("fecha_entrega")
                    .IsFixedLength();

                entity.HasOne(d => d.IdTallaNavigation)
                   .WithMany(p => p.Apartados)
                   .HasForeignKey(d => d.IdTalla)
                   .HasConstraintName("FK_Articulos_Tallas");

                entity.HasOne(d => d.IdArticuloNavigation)
                   .WithMany(p => p.Apartados)
                   .HasForeignKey(d => d.idArticulo)
                   .HasConstraintName("FK_Apartados_Articulos");

                entity.HasOne(d => d.IdClienteNavigation)
                .WithMany(p => p.Apartados)
                .HasForeignKey(d => d.IdEmpleado)
                .HasConstraintName("FK_Apartados_clientes");

            });

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

                entity.Property(e => e.Imagen)
                    .IsUnicode(false)
                    .HasColumnName("imagen");

                entity.Property(e => e.Precio).HasColumnName("precio");

                entity.Property(e => e.Sku)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sku");

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

            modelBuilder.Entity<Caja>(entity =>
            {
                entity.HasKey(e => e.IdCaja)
                    .HasName("PK__Caja__C71E2476ACB9E461");

                entity.ToTable("Caja");

                entity.Property(e => e.IdCaja).HasColumnName("id_caja");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.FechaCierre)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_cierre");

                entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");

                entity.Property(e => e.Monto)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("monto");

                entity.Property(e => e.MontoCierre)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("monto_cierre");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.Cajas)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Caja__Empleado");
            });

            modelBuilder.Entity<CambiosDevolucione>(entity =>
            {
                entity.HasKey(e => e.IdCambioDevolucion)
                    .HasName("PK__CambiosD__DB3A8A4727E7EC86");

                entity.Property(e => e.IdCambioDevolucion).HasColumnName("id_cambio_devolucion");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.IdVenta).HasColumnName("id_venta");

                entity.Property(e => e.NoArticulos).HasColumnName("no_articulos");

                entity.Property(e => e.Subtotal)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("subtotal");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("total");

                entity.HasOne(d => d.IdVentaNavigation)
                    .WithMany(p => p.CambiosDevoluciones)
                    .HasForeignKey(d => d.IdVenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CambiosDevoluciones_Ventas");
            });

            modelBuilder.Entity<CambiosDevolucionesArticulo>(entity =>
            {
                entity.HasKey(e => e.IdCambioArticulo)
                    .HasName("PK__CambiosD__9DD2439FE364C556");

                entity.Property(e => e.IdCambioArticulo).HasColumnName("id_cambio_articulo");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.Deducible)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("deducible");

                entity.Property(e => e.Estado)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.IdArticulo).HasColumnName("id_articulo");

                entity.Property(e => e.IdCambioDevolucion).HasColumnName("id_cambio_devolucion");

                entity.Property(e => e.IdVentaArticulo).HasColumnName("id_venta_articulo");

                entity.Property(e => e.MotivoCambio)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("motivo_cambio");

                entity.Property(e => e.PrecioActual)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("precio_actual");

                entity.Property(e => e.PrecioAnterior)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("precio_anterior");

                entity.HasOne(d => d.IdArticuloNavigation)
                    .WithMany(p => p.CambiosDevolucionesArticulos)
                    .HasForeignKey(d => d.IdArticulo)
                    .HasConstraintName("FK__CambiosDevolucionesArticulos_Articulos");

                entity.HasOne(d => d.IdCambioDevolucionNavigation)
                    .WithMany(p => p.CambiosDevolucionesArticulos)
                    .HasForeignKey(d => d.IdCambioDevolucion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CambiosDevolucionesArticulos_CambiosDevoluciones");

                entity.HasOne(d => d.IdVentaArticuloNavigation)
                    .WithMany(p => p.CambiosDevolucionesArticulos)
                    .HasForeignKey(d => d.IdVentaArticulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CambiosDevolucionesArticulos_VentaArticulos");
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

            modelBuilder.Entity<CatCliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente);

                entity.Property(e => e.IdCliente).HasColumnName("id_cliente");

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(50)
                    .HasColumnName("apellido_materno")
                    .IsFixedLength();

                entity.Property(e => e.ApellidoPaterno)
                    .HasMaxLength(50)
                    .HasColumnName("apellido_paterno")
                    .IsFixedLength();

                entity.Property(e => e.Direccion)
                    .HasMaxLength(50)
                    .HasColumnName("direccion")
                    .IsFixedLength();

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .HasColumnName("nombre")
                    .IsFixedLength();

                entity.Property(e => e.Telefono1)
                    .HasMaxLength(50)
                    .HasColumnName("telefono1")
                    .IsFixedLength();

                entity.Property(e => e.Telefono2)
                    .HasMaxLength(50)
                    .HasColumnName("telefono2")
                    .IsFixedLength();

                entity.Property(e => e.Visible).HasColumnName("visible");
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
                    .HasMaxLength(50)
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

            modelBuilder.Entity<MaterialesUbicacione>(entity =>
            {
                entity.HasKey(e => e.IdMaterialUbicacion)
                    .HasName("PK__Material__ED06EAA728964921");

                entity.Property(e => e.IdMaterialUbicacion).HasColumnName("id_material_ubicacion");

                entity.Property(e => e.IdMaterial).HasColumnName("id_material");

                entity.Property(e => e.IdUbicacion).HasColumnName("id_ubicacion");

                entity.HasOne(d => d.IdMaterialNavigation)
                    .WithMany(p => p.MaterialesUbicaciones)
                    .HasForeignKey(d => d.IdMaterial)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MaterialesUbicaciones_Materiales");

                entity.HasOne(d => d.IdUbicacionNavigation)
                    .WithMany(p => p.MaterialesUbicaciones)
                    .HasForeignKey(d => d.IdUbicacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MaterialesUbicaciones_CatUbicaciones");
            });

            modelBuilder.Entity<PagoApartado>(entity => 
            {
                    entity.HasKey(e => e.IdPagoApartado);
                    entity.Property(e => e.IdPagoApartado).HasColumnName("id_pagoApartado");

                     entity.Property(e => e.IdApartado)
                    .HasMaxLength(50)
                    .HasColumnName("id_apartado")
                    .IsFixedLength();

                entity.Property(e => e.IdArticulo)
                    .HasMaxLength(50)
                    .HasColumnName("id_articulo")
                    .IsFixedLength();

                entity.Property(e => e.IdCliente)
                    .HasMaxLength(50)
                    .HasColumnName("id_cliente")
                    .IsFixedLength();

                entity.Property(e => e.Cantidad)
                    .HasMaxLength(50)
                    .HasColumnName("cantidad")
                    .IsFixedLength();


                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status")
                    .IsFixedLength();

                entity.Property(e => e.Fecha)
                    .HasMaxLength(50)
                    .HasColumnName("fecha")
                    .IsFixedLength();
            }
                
                );

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

                entity.Property(e => e.pc)
                 .HasMaxLength(50)
                 .IsUnicode(false)
                 .HasColumnName("pc");

                entity.Property(e => e.ubicacion)
                 .HasMaxLength(50)
                 .IsUnicode(false)
                 .HasColumnName("ubicacion");


                entity.Property(e => e.impresora)
                 .HasMaxLength(50)
                 .IsUnicode(false)
                 .HasColumnName("impresora");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK_Users_Rol");
            });

            modelBuilder.Entity<Venta>(entity =>
            {
                entity.HasKey(e => e.IdVenta)
                    .HasName("PK__Ventas__459533BF081C1F5A");

                entity.Property(e => e.IdVenta).HasColumnName("id_venta");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.IdCaja).HasColumnName("id_caja");

                entity.Property(e => e.NoArticulos).HasColumnName("no_articulos");

                entity.Property(e => e.NoTicket)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("no_ticket");

                entity.Property(e => e.Subtotal)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("subtotal");

                entity.Property(e => e.TipoPago)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tipo_pago");

                entity.Property(e => e.TipoVenta)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tipo_venta");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("total");

                entity.HasOne(d => d.IdCajaNavigation)
                    .WithMany(p => p.Venta)
                    .HasForeignKey(d => d.IdCaja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ventas__Caja");
            });

            modelBuilder.Entity<VentaArticulo>(entity =>
            {
                entity.HasKey(e => e.IdVentaArticulo)
                    .HasName("PK__VentaArt__A3985937EAF0CFF6");

                entity.Property(e => e.IdVentaArticulo).HasColumnName("id_venta_articulo");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.IdArticulo).HasColumnName("id_articulo");

                entity.Property(e => e.IdVenta).HasColumnName("id_venta");

                entity.Property(e => e.PrecioUnitario)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("precio_unitario");

                entity.Property(e => e.Subtotal)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("subtotal");

                entity.HasOne(d => d.IdArticuloNavigation)
                    .WithMany(p => p.VentaArticulos)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__VentaArticulos_Articulos");

                entity.HasOne(d => d.IdVentaNavigation)
                    .WithMany(p => p.VentaArticulos)
                    .HasForeignKey(d => d.IdVenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__VentaArticulos_Ventas");
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
                    .HasConstraintName("FK_VistaRol_Vistas");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
