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
            return Context.Solicitudes.Include(s => s.Solicitante).Include(s => s.Obra).Include(s => s.Aprovador).FirstOrDefault(s => s.Id == id);
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

        public List<Solicitud> TomarTodos2()
        {
            return Context.Solicitudes.Include(s => s.Obra).Include(s => s.Solicitante).Include(s => s.Aprovador).ToList();
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

        internal Solicitud CrearSolicitud(int idObra, int idSolicitante, string Comentario)
        {
            Solicitud solicitud = new Solicitud();
            solicitud.IdObra = idObra;
            solicitud.IdUsuario = idSolicitante;
            solicitud.Comentario = Comentario;
            solicitud.Estado = Estado.Solicitado;
            return solicitud;
        }

        //internal void AsignarMat(SolicitudMaterial solicitudMaterial)
        //{
        //    Fachada fachada = new Fachada();
        //    if (solicitudMaterial != null)
        //    {
        //        solicitudMaterial.Material = fachada.BuscarMaterial(solicitudMaterial.IdMaterial);
        //        Context.SaveChanges();
        //    }
        //}

        internal IEnumerable<Solicitud> BuscarSolicitudesPendientes()
        {
            IEnumerable<Solicitud> solicitudesPendientes = Context.Solicitudes.Include(s => s.Obra).Include(s => s.Solicitante).Include(s => s.Aprovador).Where(sp => sp.Estado == Estado.Solicitado);

            //foreach (Solicitud soli in solicitudesPendientes)
            //{
            //    soli.Obra = fachada.BuscarObra(soli.IdObra);
            //    soli.Solicitante = fachada.BuscarUsuarioObra(soli.IdUsuario);
            //}

            return solicitudesPendientes;
        }

        internal IEnumerable<Solicitud> BuscarSolicitudesAprobadas()
        {
            return Context.Solicitudes.Where(sp => sp.Estado == Estado.Aprobado);
        }

        internal IEnumerable<Solicitud> BuscarSolicitudesRechazadas()
        {
            return Context.Solicitudes.Where(sp => sp.Estado == Estado.Rechazado);
        }

        internal IEnumerable<Solicitud> BuscarSolicitudesRecibido()
        {
            return Context.Solicitudes.Where(sp => sp.Estado == Estado.Rechazado);
        }

        internal void ConfigurarMaterial(IEnumerable<SolicitudMaterial> laSolicitudConMateriales, Dictionary<int, int> materialesSeleccionadosConCantidad)
        {
            try
            {

                foreach (SolicitudMaterial sol in laSolicitudConMateriales)
                {
                    bool estaMaterial = false;
                    foreach (var mat in materialesSeleccionadosConCantidad)
                    {
                        if (mat.Key == sol.IdMaterial)
                        {
                            sol.Cantidad = mat.Value;
                            estaMaterial = true;
                        }
                    }
                    if (!estaMaterial)
                    {
                        Context.SolicitudesMateriales.Remove(sol);
                        // Context.SaveChanges();
                    }
                }
            }
            catch (SolicitudException se)
            {
                throw new SolicitudException(se.Message);
            }
        }

        internal void AceptarSolicitud(Solicitud solicitud, UDeOficina aprobador)
        {
            try
            {
                if (solicitud.Estado == Estado.Solicitado)
                {
                    solicitud.IdUDeOficina = aprobador.Id;
                    solicitud.Estado = Estado.Aprobado;
                    Context.SaveChanges();

                }
                else
                {
                    throw new SolicitudException("No puede aceptar o rechazar una solicitud que ya fue aceptada o rechazada");
                }

            }
            catch (SolicitudException se)
            {
                throw new SolicitudException(se.Message);

            }
        }

        internal void RechazarSolicitud(Solicitud solicitud, UDeOficina rechazador)
        {
            solicitud.IdUDeOficina = rechazador.Id;
            solicitud.Estado = Estado.Rechazado;
            Context.SaveChanges();
        }
        internal void ConfirmarSolicitud(Solicitud solicitud, Usuario logueado)
        {
           
                List<SolicitudMaterial> materialesSolicitud = MaterialesDeSolicitud(solicitud.Id).ToList();
                foreach (SolicitudMaterial sm in materialesSolicitud)
                {
                    ObraMaterial obraMaterialExistente = ExisteMaterialEnObra(solicitud, sm);
                    if (obraMaterialExistente != null)
                    {
                        obraMaterialExistente.Stock += sm.Cantidad;
                    }
                    else
                    {
                        ObraMaterial om = new ObraMaterial();
                        om.IdMaterial = sm.IdMaterial;
                        om.IdObra = solicitud.IdObra;
                        om.Stock = sm.Cantidad;
                        om.Validar();
                        Context.ObrasMateriales.Add(om);
                    }
                }
                solicitud.Estado = Estado.Recibido;
                Context.SaveChanges();
           
        }

        private ObraMaterial ExisteMaterialEnObra(Solicitud solicitud, SolicitudMaterial sm)
        {
            return Context.ObrasMateriales.Where(o => o.IdMaterial == sm.IdMaterial && o.IdObra == solicitud.IdObra).FirstOrDefault();
        }


        internal List<Solicitud> BuscarSolicitudPendientesLista()
        {
            return Context.Solicitudes.Include(s => s.Obra).Where(sp => sp.Estado == Estado.Solicitado).ToList();
        }

        internal List<Solicitud> BuscarSolicitudesAprobadasParaUnUObra(string? nomObrero)
        {
            Usuario u = this.BuscarUsuarioXNombreU(nomObrero);

            return Context.Solicitudes.Include(s => s.Obra).Where(sp => sp.Estado == Estado.Aprobado && sp.Obra.IdACargo == u.Id).ToList();
        }

        private Usuario BuscarUsuarioXNombreU(string? nomObrero)
        {
            return Context.Usuarios.Where(u => u.NombreUsuario == nomObrero).FirstOrDefault();
        }

        internal List<Solicitud> BuscarSolicitudConfirmada()
        {
            return Context.Solicitudes.Include(s => s.Obra).Where(sp => sp.Estado == Estado.Recibido).ToList();
        }

        internal void CambiarEstadoAVisto(Solicitud solicitud)
        {
            solicitud.Estado = Estado.Visto;
            Context.SaveChanges();
        }
    }
}
