using LogicaAccesoDatos.EF;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using LogicaNegocio.ViewModel;
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
            if (direccion == "<<A INGRESAR>>")
            {
                return null;
            }
            var Retorno = Context.Obras.Where(o => o.Direccion == direccion).FirstOrDefault();
            return Retorno;
        }

        public IEnumerable<Obra> BuscarPorDireccion(string direccion)
        {
            var Retorno = Context.Obras.Where(o => o.Direccion == direccion).ToList();
            return Retorno;
        }

        public Obra BuscarPorNombre(string nombre)
        {
            var Retorno = Context.Obras.Where(o => o.Nombre == nombre).FirstOrDefault();
            return Retorno;
        }

        public void Eliminar(Obra obra)
        {
            if (obra == null)
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
                nuevaObra.Validar();
                Obra obra = this.Buscar(nuevaObra.IdObra);
                Obra obraMismoNom;
                obraMismoNom = this.ObraPorNombre(nuevaObra.Nombre); //Para comparar id sino en caso de editar una obra y no editar el nombre tira nombre repetido igual

                if (obra == null)
                {
                    throw new ObraException("No se encontró la obra a modificar.");
                }
                if (obraMismoNom != null && nuevaObra.IdObra != obraMismoNom.IdObra)
                {
                    throw new ObraException("El nombre de obra ingresado ya está en uso. Elegir otro.");
                }
                if (this.ObraPorDireccion(nuevaObra.Direccion) != null)
                {
                    throw new ObraException("La ubicación de obra ingresada coincide con una existente. Elegir otra.");
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
            if (nombre != null)
            {
                obras = obras.Where(o => o.Nombre.Contains(nombre));
            }
            if (direccion != null)
            {
                obras = obras.Where(o => o.Nombre.Contains(nombre));
            }
            if (finalizada != null)
            {
                obras = obras.Where(o => o.Finalizada == finalizada);
            }
            return obras.ToList();
        }

        public IEnumerable<Obra> TomarTodos()
        {
            return Context.Obras.ToList();
        }

        internal IEnumerable<Obra> TomarObrasDeUnUsuarioObra(string? nomUsuarioObra)
        {
            Usuario u = this.BuscarUsuarioXNombreU(nomUsuarioObra);

            return Context.Obras.Where(o => o.IdACargo == u.Id).ToList();

        }
        private Usuario BuscarUsuarioXNombreU(string? nomObrero)
        {
            return Context.Usuarios.Where(u => u.NombreUsuario == nomObrero).FirstOrDefault();
        }

        public Obra Buscar(int id)
        {
            Obra retorno = null;
            foreach (Obra obra in Context.Obras)
            {
                if (obra.IdObra == id)
                {
                    retorno = obra; break;
                }
            }
            return retorno;
            //No estaba funcionando la consulta. Nose porque
           // return Context.Obras.Where(o=> o.IdObra == id).Include(o=>o.UsuarioACargo).FirstOrDefault();
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
            if (retorno.Count > 0)
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
                    retorno.Add(BuscarProveedor((int)s.IdProveedor), 1);
                }
                else
                {
                    var = new KeyValuePair<Proveedor, int>(BuscarProveedor((int)s.IdProveedor), var.Value + 1);
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
                if (var.Key == null)
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

        internal IEnumerable<ObraMaterial> MaterialesDeObra(int idObra)
        {

            return Context.ObrasMateriales.Where(m => m.Obra.IdObra == idObra).Include(o => o.Material).Include(o => o.Obra);
        }

        internal void ConsumirMateriales(List<MaterialConsumoViewModel>? item, Obra obra)
        {
            List<ObraMaterial> materialesObra = MaterialesDeObra(obra.IdObra).ToList();

            foreach (MaterialConsumoViewModel m in item)
            {
                foreach (ObraMaterial om in materialesObra)
                {
                    if (m.Material.Id == om.Material.Id)
                    {
                        if (m.Cantidad <= om.Stock)
                        {
                            om.Stock -= m.Cantidad;
                        }
                    }
                }
            }
            Context.SaveChanges();
        }
        internal List<ObraMaterial> AlertarStockDeMaterialesEnObra(Obra obra)
        {
            List<ObraMaterial> materialesAlertar = new List<ObraMaterial>();
            List<ObraMaterial> materialesObra = MaterialesDeObra(obra.IdObra).ToList();
            foreach (ObraMaterial om in materialesObra)
            {
                if (om.Material.BarreraDeStock > om.Stock)
                {
                    materialesAlertar.Add(om);
                }

            }
            return materialesAlertar;
        }
        internal List<ObraMaterial> AlertarStockDeMaterialesTodasObras()
        {
            List<ObraMaterial> materialesAlertar = new List<ObraMaterial>();
            List<Obra> obras = this.TomarTodos().ToList();
            foreach (Obra o in obras)
            {
                materialesAlertar.AddRange(this.AlertarStockDeMaterialesEnObra(o));
            }
            return materialesAlertar;
        }
        internal List<ObraMaterial> AlertarStockDeMaterialesTodasObrasACargo(string? nomObrero)
        {
            List<ObraMaterial> materialesAlertar = new List<ObraMaterial>();
            List<Obra> obras = this.TomarObrasDeUnUsuarioObra(nomObrero).ToList();
            foreach (Obra o in obras)
            {
                materialesAlertar.AddRange(this.AlertarStockDeMaterialesEnObra(o));
            }
            return materialesAlertar;
        }


        internal bool MaterialesCheckStock(List<MaterialConsumoViewModel>? item, Obra obra)
        {
            List<ObraMaterial> materialesObra = MaterialesDeObra(obra.IdObra).ToList();
            foreach (MaterialConsumoViewModel m in item)
            {
                foreach (ObraMaterial om in materialesObra)
                {
                    if (m.Material.Id == om.Material.Id)
                    {
                        if (m.Cantidad > om.Stock)
                        {
                            return true;
                        }

                    }
                }
            }
            return false;
        }


        public void AsignacionHorasLluvia(Obra obra, int horas, DateTime dia)
        {
            if (horas <= 0 || horas > 24)
            {
                throw new ObraException("Ingrese una cantidad de horas valida.");
            }
            TimeSpan diff = dia - DateTime.Today;
            if (diff.Days > 0)
            {
                throw new ObraException("No se pueden asignar horas lluvia a un dia posterior a hoy.");
            }
            if (dia.Year < 1000)
            {
                throw new ObraException("Seleccione una fecha.");

            }
            List<ObraEmpleado> empleadosObra = GetEmpleadosObra(obra);
            foreach (ObraEmpleado oe in empleadosObra)
            {
                Marca m = MarcaDelDia(oe, dia);
                if (m != null) //El empleado trabajó ese día
                {
                    m.HorasLluvia = horas;
                }
            }
            Context.SaveChanges();
        }

        private Marca MarcaDelDia(ObraEmpleado oe, DateTime dia)
        {
            return Context.Marcas.Where(m => m.Entrada.Day == dia.Day && m.Salida.Day == dia.Day && m.IdEmpleado == oe.IdEmpleado).FirstOrDefault();
        }

        public int HorasEmpleadoEnObra(ObraEmpleado oe, DateTime fechaDesde, DateTime fechaHasta)
        {
            int horas = 0;
            List<Marca> marcasEmpleado = this.marcasEmpleado(oe, fechaDesde, fechaHasta);

            foreach (Marca m in marcasEmpleado)
            {
                int marcaEnHoras = m.HorasTrabajadas();
                horas += marcaEnHoras;
            }

            return horas;
        }

        public int HorasLluviaEmpleadoEnObra(ObraEmpleado oe, DateTime fechaDesde, DateTime fechaHasta)
        {
            int horas = 0;
            List<Marca> marcasEmpleado = this.marcasEmpleado(oe, fechaDesde, fechaHasta);

            foreach (Marca m in marcasEmpleado)
            {
                int marcaEnHoras = m.HorasLluvia;
                horas += marcaEnHoras;
            }

            return horas;
        }

        private List<Marca> marcasEmpleado(ObraEmpleado oemp, DateTime fechaDesde, DateTime fechaHasta)
        {
            return Context.Marcas.Where(ma => ma.Empleado.IdObra == oemp.IdObra
            && ma.Empleado.IdEmpleado == oemp.IdEmpleado
            && ma.Entrada.Day == fechaDesde.Day
            && ma.Salida.Day == fechaHasta.Day).ToList();
        }

        public List<ObraEmpleado> GetEmpleadosObra(Obra obra)
        {
            return Context.ObrasEmpleados.Where(oe => oe.IdObra == obra.IdObra).Include(oe => oe.Empleado).Include(oe => oe.Empleado.TipoEmpleado).Include(oe => oe.Obra).ToList();
        }

        internal ObraEmpleado EmpleadoObra(int idEmpleado, int idObra)
        {
            return Context.ObrasEmpleados.Where(oe => oe.IdObra == idObra && oe.IdEmpleado == idEmpleado).Include(oe => oe.Empleado).Include(oe => oe.Obra).Include(oe => oe.Empleado.TipoEmpleado).FirstOrDefault();
        }

        public void DarEgreso(ObraEmpleado empleado, DateTime fecha)
        {
            try
            {
                empleado = this.EmpleadoObra(empleado.IdEmpleado, empleado.IdObra);
                empleado.FechaEgreso = fecha;
                Context.SaveChanges();
            }
            catch (EmpleadoException ee)
            {
                throw new EmpleadoException(ee.Message);
            }
        }

        private bool ExisteEmpleadoEnObra(ObraEmpleado oe)
        {
            return Context.ObrasEmpleados.Where(e => e.IdObra == oe.IdObra && e.IdEmpleado == oe.IdEmpleado).Any();
        }

        internal double CalcularTotalLiquidacion(List<ObraEmpleadoLiquidacionViewModel> vm)
        {
           double total = 0;

            foreach (ObraEmpleadoLiquidacionViewModel emp in vm)
            {
                total += emp.Liquidacion;
            }

            return total;
        }
    }
}
