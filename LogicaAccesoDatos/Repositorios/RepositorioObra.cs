using LogicaAccesoDatos.EF;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LogicaAccesoDatos.Repositorios
{
    public class RepositorioObra : IRepositorioObra
    {
        public ProyectoContext Context { get; set; }

        public RepositorioObra()
        {
            this.Context = new ProyectoContext();
        }
        public void Agregar(Obra item)
        {

            item.Validar();
            if (this.ObraPorNombre(item.Nombre) != null)
            {   
                throw new ObraException("El nombre de obra ingresado ya está en uso. Elegir otro.");
            }
            if (this.ObraPorDireccion(item.Direccion) != null)
            {
                throw new ObraException("La ubicación de obra ingresada coincide con una existente. Elegir otra.");
            }
            Context.Obras.Add(item);
            Context.SaveChanges();
        }

        public Obra ObraPorNombre(string nombre)
        {
            var Retorno = Context.Obras.Where(o => o.Nombre == nombre).FirstOrDefault();
            return Retorno;
        }

        public Obra ObraPorDireccion(string direccion)
        {
            var Retorno = Context.Obras.Where(o => o.Direccion == direccion).FirstOrDefault();
            return Retorno;
        }

        public IEnumerable<Obra> BuscarPorDireccion(string direccion)
        {
            var Retorno = Context.Obras.Where(o => o.Direccion == direccion).ToList();
            return Retorno;
        }

        public IEnumerable<Obra> BuscarPorNombre(string nombre)
        {
            var Retorno = Context.Obras.Where(o => o.Nombre == nombre).ToList();
            return Retorno;
        }

        public void Eliminar(Obra obra)
        {
            if(obra == null)
            {
                throw new ObraException("No se puede eliminar una obra nula.");
            }
            if (TieneSolicitudesPendientes(obra))
            {
                throw new ObraException("No se puede eliminar la obra, tiene solicitudes de material en estado pendiente.");
            }
            Context.Obras.Remove(obra);
            Context.SaveChanges();
        }

        public void FinalizarObra(Obra obra)
        {
            if (obra == null)
            {
                throw new ObraException("No se puede finalizar una obra nula.");
            }
            if (TieneSolicitudesPendientes(obra))
            {
                throw new ObraException("No se puede cerrar la obra, tiene solicitudes de material en estado pendiente.");
            }
            try
            {
                obra.FinalizarObra();
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new ObraException($"No se puede finalizar la obra {obra.Nombre}: {e.Message}");
            }
        }

      

        public void Modificar(Obra nuevaObra)
        {
            try
            {
                //nuevaObra.Validar();
                Obra obra = this.Buscar(nuevaObra.IdObra);
                if (obra == null)
                {
                    throw new ObraException("No se encontró la obra a modificar.");
                }

                obra.Nombre = nuevaObra.Nombre;
                obra.Direccion = nuevaObra.Direccion;
                obra.FechaInicio = nuevaObra.FechaInicio;
                obra.FechaFinalizacion = nuevaObra.FechaFinalizacion;
                obra.Cronograma = nuevaObra.Cronograma;
                obra.TipoCronograma = nuevaObra.TipoCronograma;
                obra.NombreCronograma = nuevaObra.NombreCronograma;
                Context.Entry(obra).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ObraException(ex.Message);
            }
        }

        public IEnumerable<Obra> ObrasFiltradas(string? nombre, string? direccion, bool? finalizada)
        {
            IEnumerable<Obra> obras = this.TomarTodos();
            if(nombre != null)
            {
                obras = obras.Where(o => o.Nombre.Contains(nombre));
            }
            if(direccion != null)
            {
                obras = obras.Where(o => o.Nombre.Contains(nombre));
            }
            if(finalizada != null)
            {
                obras = obras.Where(o => o.Finalizada == finalizada);
            }
            return obras.ToList();
        }

        public IEnumerable<Obra> TomarTodos()
        {
            return Context.Obras.ToList();
        }

        public Obra Buscar(int id)
        {
            return Context.Obras.Find(id);
        }

        public Material MaterialMasSolicitado(int IdObra)
        {
            Dictionary<Material, int> retorno = new Dictionary<Material, int>();
            IEnumerable<Solicitud> solicitudesObra = Context.Solicitudes.Where(s => s.IdObra == IdObra);
            foreach (Solicitud s in solicitudesObra)
            {
                IEnumerable<SolicitudMaterial> MaterialesSolicitados = Context.SolicitudesMateriales.Where(m => m.IdSolicitud == s.Id);
                foreach (SolicitudMaterial m in MaterialesSolicitados)
                {
                    KeyValuePair<Material, int> var = retorno.First(r => r.Key.Id == m.IdMaterial);
                    if (var.Key != null)
                    {
                        retorno.Add(m.Material, m.Cantidad);
                    }
                    else
                    {
                        var = new KeyValuePair<Material, int>(var.Key, var.Value + m.Cantidad);
                    }
                }
            }
            if(retorno.Count > 0)
            {
                return retorno.Max().Key;
            }
            else
            {
                return null;
            }
        }
        public Material MaterialMenosSolicitado(int IdObra)
        {
            Dictionary<Material, int> retorno = new Dictionary<Material, int>();
            IEnumerable<Solicitud> solicitudesObra = Context.Solicitudes.Where(s => s.IdObra == IdObra);
            foreach (Solicitud s in solicitudesObra)
            {
                IEnumerable<SolicitudMaterial> MaterialesSolicitados = Context.SolicitudesMateriales.Where(m => m.IdSolicitud == s.Id);
                foreach (SolicitudMaterial m in MaterialesSolicitados)
                {
                    KeyValuePair<Material, int> var = retorno.First(r => r.Key.Id == m.IdMaterial);
                    if (var.Key != null)
                    {
                        retorno.Add(m.Material, m.Cantidad);
                    }
                    else
                    {
                        var = new KeyValuePair<Material, int>(var.Key, var.Value + m.Cantidad);
                    }
                }
            }
            if (retorno.Count > 0)
            {
                return retorno.Min().Key;
            }
            else
            {
                return null;
            }
        }
        public Proveedor ProveedorMasComun(int IdObra)
        {
            Dictionary<Proveedor, int> retorno = new Dictionary<Proveedor, int>();
            IEnumerable<Solicitud> solicitudesObra = Context.Solicitudes.Where(s => s.IdObra == IdObra);
            foreach (Solicitud s in solicitudesObra)
            {
                KeyValuePair<Proveedor, int> var = retorno.First(r => r.Key.Id == s.IdProveedor);
                if (var.Key != null)
                {
                    retorno.Add(BuscarProveedor(s.IdProveedor), 1);
                }
                else{
                    var = new KeyValuePair<Proveedor, int>(BuscarProveedor(s.IdProveedor), var.Value + 1);
                }
            }
            if (retorno.Count > 0)
            {
                return retorno.Max().Key;
            }
            else
            {
                return null;
            }
        }
        public Usuario SolicitanteMasComun(int IdObra)
        {
            Dictionary<Usuario, int> retorno = new Dictionary<Usuario, int>();
            IEnumerable<Solicitud> solicitudesObra = Context.Solicitudes.Where(s => s.IdObra == IdObra);
            foreach (Solicitud s in solicitudesObra)
            {
                KeyValuePair<Usuario, int> var = retorno.First(r => r.Key == s.Solicitante);
                if (var.Key != null)
                {
                    retorno.Add(s.Solicitante, 1);
                }
                else
                {
                    var = new KeyValuePair<Usuario, int>(s.Solicitante, var.Value + 1);
                }
            }
            if (retorno.Count > 0)
            {
                return retorno.Max().Key;
            }
            else
            {
                return null;
            }
        }
        public UDeOficina AprobadorMasComun(int IdObra)
        {
            Dictionary<UDeOficina, int> retorno = new Dictionary<UDeOficina, int>();
            IEnumerable<Solicitud> solicitudesObra = Context.Solicitudes.Where(s => s.IdObra == IdObra);
            foreach (Solicitud s in solicitudesObra)
            {
                KeyValuePair<UDeOficina, int> var = retorno.First(r => r.Key == s.Aprovador);
                if (var.Key != null)
                {
                    retorno.Add(s.Aprovador, 1);
                }
                else
                {
                    var = new KeyValuePair<UDeOficina, int>(s.Aprovador, var.Value + 1);
                }
            }
            if (retorno.Count > 0)
            {
                return retorno.Max().Key;
            }
            else
            {
                return null;
            }
        }
        private Proveedor BuscarProveedor(int idProveedor)
        {
            return Context.Proveedores.Find(idProveedor);
        }

        private bool TieneSolicitudesPendientes(Obra obra)
        {
            return Context.Solicitudes.Where(s => s.Obra.IdObra == obra.IdObra && s.Estado == Estado.Solicitado).Any();
        }
    }
}
