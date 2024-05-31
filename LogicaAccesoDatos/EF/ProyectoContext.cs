using LogicaNegocio.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAccesoDatos.EF
{
    public class ProyectoContext : DbContext
    {
        public DbSet<Obra> Obras { get; set; }
        public DbSet<Material> Materiales { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Plano> Planos { get; set; }
        public DbSet<TipoEmpleado> TiposEmpleados { get; set; }
        public DbSet<TipoPlano> TiposPlanos { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<Dia> Dias { get; set; }
        public DbSet<ObraEmpleado> ObrasEmpleados { get; set; }
        public DbSet<SolicitudMaterial> SolicitudesMateriales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string cadenaConexion =
                @"SERVER=(localdb)\MSsqlLocaldb;
                DATABASE=ProyectoIntegrador9; 
                INTEGRATED SECURITY=TRUE;
                ENCRYPT=False"; //Puede evitar problemas si no hay un certificado y se usa SSL
            optionsBuilder.UseSqlServer(cadenaConexion)
                .EnableDetailedErrors();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Obra>()
            .HasOne(o => o.UsuarioACargo)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction); //LAS OBRAS TIENEN UN USUARIO A CARGO. LOS USUARIOS A CARGO TIENEN MUCHAS OBRAS. CUANDO SE BORRE OBRA / USUARIO NO BORRAR EL OTRO, dejar null. Esa es la idea.
        }
    }
}
