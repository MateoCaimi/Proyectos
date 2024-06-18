using LogicaAccesoDatos.EF;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAccesoDatos.Repositorios
{
    public class RepositorioSolicitud : IRepositorioSolicitud
    {
      
        public ProyectoContext Context { get; set; }

        public RepositorioSolicitud()
        {
            this.Context = new ProyectoContext();
               
        }

        public void Agregar(Solicitud item)
        {
            item.Validar();
            Context.Solicitudes.Add(item);
            Context.SaveChanges();
        }

        public Solicitud Buscar(int id)
        {
            return Context.Solicitudes.Include(s => s.Solicitante).Include(s => s.Obra).FirstOrDefault(s =>  s.Id == id); 
        }

        public void Eliminar(Solicitud item)
        {
            throw new NotImplementedException();
        }

        public void Modificar(Solicitud item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Solicitud> TomarTodos()
        {
            return Context.Solicitudes.ToList();
        }

        public IEnumerable<Solicitud> SolicitudesDeObra(int idObra)
        {
            return Context.Solicitudes.Include(s => s.Solicitante).Include(s => s.Obra).Where(s => s.IdObra == idObra);
        }

        internal void AgregarSolicitudMateriales(Solicitud solicitud, List<SolicitudMaterial>? item)
        {
            foreach (SolicitudMaterial sm in item)
            {
                sm.IdSolicitud = solicitud.Id;
                sm.Material = null;
            }
            foreach (SolicitudMaterial sm in item)
            {
                sm.Validar();
                Context.SolicitudesMateriales.Add(sm);
                Context.SaveChanges();
            }
        }

        internal IEnumerable<SolicitudMaterial> MaterialesDeSolicitud(int id)
        {
            return Context.SolicitudesMateriales.Include(sm => sm.Material).Where(sm => sm.IdSolicitud == id);
        }

        internal Solicitud CrearSolicitud(int idObra, int idSolicitante)
        {
            Solicitud solicitud = new Solicitud();
            solicitud.IdObra = idObra;
            solicitud.IdUsuario = idSolicitante;
            solicitud.Estado = Estado.Solicitado;
            return solicitud;
        }

        internal void AsignarMat(SolicitudMaterial solicitudMaterial)
        {
            Fachada fachada = new Fachada();
            if (solicitudMaterial != null)
            {
                solicitudMaterial.Material = fachada.BuscarMaterial(solicitudMaterial.IdMaterial);
                Context.SaveChanges();
            }
        }
    }
}
