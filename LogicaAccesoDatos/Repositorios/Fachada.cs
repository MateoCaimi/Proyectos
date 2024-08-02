using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LogicaAccesoDatos.Repositorios
{
    public class Fachada
    {
        RepositorioObra RepositorioObra { get; set; }
        RepositorioPlano RepositorioPlano { get; set; }
        RepositorioUsuario RepositorioUsuario { get; set; }
        RepositorioMaterial RepositorioMaterial { get; set; }
        RepositorioSolicitud RepositorioSolicitud { get; set; }
        RepositorioEmpleado RepositorioEmpleado { get; set; }

        public Fachada()
        {
            RepositorioObra = new RepositorioObra();
            RepositorioPlano = new RepositorioPlano();
            RepositorioUsuario = new RepositorioUsuario();
            RepositorioMaterial = new RepositorioMaterial();
            RepositorioSolicitud = new RepositorioSolicitud();
            RepositorioEmpleado = new RepositorioEmpleado();
        }
        public Obra ObraPorDireccion(string direccion)
        {
            return RepositorioObra.ObraPorDireccion(direccion);
        }
        public Obra ObraPorNombre(string nombre)
        {
            return RepositorioObra.ObraPorNombre(nombre);
        }
        public IEnumerable<Obra> ObrasFiltradas(string? nombre, string? direccion, bool? finalizada)
        {
            return RepositorioObra.ObrasFiltradas(nombre, direccion, finalizada);
        }
        public Obra BuscarPorNombre(string nombre)
        {
            return RepositorioObra.BuscarPorNombre(nombre);
        }
        public IEnumerable<Obra> BuscarPorDireccion(string direccion)
        {
            return RepositorioObra.BuscarPorDireccion(direccion);
        }
        public void FinalizarObra(Obra obra)
        {
            RepositorioObra.FinalizarObra(obra);
        }
        public Material MaterialMenosSolicitado(int IdObra)
        {
            return RepositorioObra.MaterialMenosSolicitado(IdObra);
        }
        public Material MaterialMasSolicitado(int IdObra)
        {
            return RepositorioObra.MaterialMasSolicitado(IdObra);
        }
        public Proveedor ProveedorMasComun(int IdObra)
        {
            return RepositorioObra.ProveedorMasComun(IdObra);
        }
        public Usuario SolicitanteMasComun(int IdObra)
        {
            return RepositorioObra.SolicitanteMasComun(IdObra);
        }
        public UDeOficina AprobadorMasComun(int IdObra)
        {
            return RepositorioObra.AprobadorMasComun(IdObra);
        }
        public void AgregarObra(Obra item)
        {
            RepositorioObra.Agregar(item);
        }
        public void EliminarObra(Obra item)
        {
            RepositorioObra.Eliminar(item);
        }
        public void ModificarObra(Obra item)
        {
            RepositorioObra.Modificar(item);
        }
        public Obra BuscarObra(int id)
        {
            return RepositorioObra.Buscar(id);
        }
        public IEnumerable<Obra> TomarTodasObras()
        {
            return RepositorioObra.TomarTodos();
        }
        public IEnumerable<Obra> TomarObrasDeUnUsuarioObra(string? nomUsuarioObra)
        {
            return RepositorioObra.TomarObrasDeUnUsuarioObra(nomUsuarioObra);
        }
        public IEnumerable<Plano> PlanosTotales(Obra obra)
        {
            return RepositorioPlano.PlanosTotales(obra);
        }
        public IEnumerable<Plano> PlanosFiltrados(Obra obra, int? tipo, string? nombre, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            return RepositorioPlano.PlanosFiltrados(obra, tipo, nombre, fechaDesde, fechaHasta);
        }
        public IEnumerable<Plano> PlanosPorAntiguedad(Obra obra)
        {
            return RepositorioPlano.PlanosPorAntiguedad(obra);
        }
        public IEnumerable<TipoPlano> BuscarTiposPlanos()
        {
            return RepositorioPlano.BuscarTiposPlanos();
        }
        public void AgregarPlano(Plano item)
        {
            RepositorioPlano.Agregar(item);
        }
        public void EliminarPlano(Plano item)
        {
            RepositorioPlano.Eliminar(item);
        }
        public void ModificarPlano(Plano item)
        {
            RepositorioPlano.Modificar(item);
        }
        public Plano BuscarPlano(int id)
        {
            return RepositorioPlano.Buscar(id);
        }
        public IEnumerable<Plano> TomarTodosPlanos()
        {
            return RepositorioPlano.TomarTodos();
        }
        public Usuario InicioSesion(string nomUsu, string pass)
        {
            return RepositorioUsuario.InicioSesion(nomUsu, pass);
        }
        public IEnumerable<Usuario> ObtenerUsuariosDeObra()
        {
            return RepositorioUsuario.TomarTodosDeObra();
        }

        public IEnumerable<Usuario> ObtenerUsuarios()
        {
            return RepositorioUsuario.TomarTodos();
        }

        public void AgregarUsuario(Usuario usuario)
        {
            RepositorioUsuario.Agregar(usuario);
        }

        public UDeObra BuscarUsuarioObra(int idACargo)
        {
            return (UDeObra)RepositorioUsuario.Buscar(idACargo);
        }
        public void EliminarUsuario(Usuario usuario)
        {
            RepositorioUsuario.Eliminar(usuario);
        }

        public Usuario CastearUsuario(string nombre, string nomUsuario, string pass, string tipo)
        {
            return RepositorioUsuario.CastearU(nombre, nomUsuario, pass, tipo);
        }

        public Usuario BuscarUsuario(int id)
        {
            return RepositorioUsuario.Buscar(id);
        }

        public void ModificarUsuario(Usuario u)
        {
            RepositorioUsuario.Modificar(u);
        }

        public IEnumerable<Plano> BuscarPlanosDelTipoEnObra(int idTipo, int idObra)
        {
            return RepositorioPlano.BuscarPlanosDelTipoEnObra(idTipo, idObra);
        }

        public TipoPlano BuscarTipoPlano(int idTipo)
        {
            return RepositorioPlano.BuscarTipoPlano(idTipo);
        }

        public void CambiarPass(string nombreUsuario, string contrasenia, string confirmarPass)
        {
            RepositorioUsuario.cambiarPass(nombreUsuario, contrasenia, confirmarPass);
        }

        public IEnumerable<Material> TomarTodosMateriales()
        {
            return RepositorioMaterial.TomarTodos();
        }

        public void AgregarMaterial(Material nuevoMaterial)
        {
            RepositorioMaterial.Agregar(nuevoMaterial);
        }

        public void EliminarMaterial(Material material)
        {
            RepositorioMaterial.Eliminar(material);
        }

        public Material BuscarMaterial(int id)
        {
            return RepositorioMaterial.Buscar(id);
        }

        public void ModificarMaterial(Material material)
        {
            RepositorioMaterial.Modificar(material);
        }

        public IEnumerable<Material> MaterialesFiltrados(string nombre)
        {
            return RepositorioMaterial.MaterialesFiltados(nombre);
        }

        public IEnumerable<Solicitud> SolicitudesDeObra(int idObra)
        {
            return RepositorioSolicitud.SolicitudesDeObra(idObra);
        }

        public IEnumerable<Material> TodosLosMateriales()
        {
            return RepositorioMaterial.TomarTodos();
        }

        public Usuario BuscarUsuarioXNombreU(string nombreUsuario)
        {
            return RepositorioUsuario.UsuarioPorNombreUsuario(nombreUsuario);
        }

        public List<Plano> CrearPlanosMultiples(int idObra, int idTipoPlano, List<IFormFile> postedFiles)
        {
            return RepositorioPlano.CrearPlanosMultiples(idObra, idTipoPlano, postedFiles);
        }

        public void AgregarSolicitud(Solicitud solicitud)
        {
            RepositorioSolicitud.Agregar(solicitud);
        }

        public void AgregarSolicitudMateriales(Solicitud solicitud, List<SolicitudMaterial>? item)
        {
            RepositorioSolicitud.AgregarSolicitudMateriales(solicitud, item);
        }

        public Solicitud BuscarSolicitud(int id)
        {
            return RepositorioSolicitud.Buscar(id);
        }

        public IEnumerable<SolicitudMaterial> BuscarMaterialesSolicitud(int id)
        {
            return RepositorioSolicitud.MaterialesDeSolicitud(id);
        }

        public Solicitud CrearSolicitud(int idObra, int idSolicitante)
        {
            return RepositorioSolicitud.CrearSolicitud(idObra, idSolicitante);
        }

        public void ConfigurarSolicitud(IEnumerable<SolicitudMaterial> laSolicitudConMateriales, Dictionary<int, int> materialesSeleccionadosConCantidad)
        {
            RepositorioSolicitud.ConfigurarMaterial(laSolicitudConMateriales, materialesSeleccionadosConCantidad);
        }

        public IEnumerable<Solicitud> BuscarSolicitudPendientes()
        {
            return RepositorioSolicitud.BuscarSolicitudesPendientes();
        }

        public void AceptarSolicitud(Solicitud solicitud, UDeOficina aprobador)
        {
            RepositorioSolicitud.AceptarSolicitud(solicitud, aprobador);
        }

        public void RechazarSolicitud(Solicitud solicitud, UDeOficina rechazador)
        {
            RepositorioSolicitud.RechazarSolicitud(solicitud, rechazador);
        }

        public List<Solicitud> BuscarSolicitudPendientesLista()
        {
            return RepositorioSolicitud.BuscarSolicitudPendientesLista();
        }
        public List<Solicitud> BuscarSolicitudAprobadasParaUnUObra(string? nomObrero)
        {
           return RepositorioSolicitud.BuscarSolicitudesAprobadasParaUnUObra(nomObrero);
        }

        public List<Solicitud> BuscarSolicitudConfirmadasLista()
        {
            return RepositorioSolicitud.BuscarSolicitudConfirmada();
        }

        public void ConfirmarSolicitud(Solicitud solicitud, Usuario logueado)
        {
            RepositorioSolicitud.ConfirmarSolicitud(solicitud, logueado);
        }

        public IEnumerable<ObraMaterial> MaterialesDeObra(int idObra)
        {
            return RepositorioObra.MaterialesDeObra(idObra);
        }

        public void ConsumirMateriales(List<MaterialConsumoViewModel>? item, Obra obra)
        {
            RepositorioObra.ConsumirMateriales(item, obra);
        }
        
        public IEnumerable<Empleado> TomarTodosEmpleados()
        {
            return RepositorioEmpleado.TomarTodos();
        }

        public Empleado BuscarEmpleado(int id)
        {
            return RepositorioEmpleado.Buscar(id);
        }

        public void ModificarEmpleado(Empleado nuevoEmpleado)
        {
            RepositorioEmpleado.Modificar(nuevoEmpleado);
        }

        public async Task<Task<string>> LlamadaClodtimes(DateTime inicio, DateTime fin)
        {
            return RepositorioEmpleado.LlamadaCloudtimes(inicio, fin);
        }

        public void AsignacionHoras(Obra obra, int horasLluvia, DateTime dia)
        {
            RepositorioObra.AsignacionHorasLluvia(obra, horasLluvia, dia);
        }

        public int HorasEmpleadoEnObra(ObraEmpleado oe, DateTime fechaDesde, DateTime fechaHasta)
        {
            return RepositorioObra.HorasEmpleadoEnObra(oe, fechaDesde, fechaHasta);
        }

        public int HorasLluviaEmpleadoEnObra(ObraEmpleado oe, DateTime fechaDesde, DateTime fechaHasta)
        {
            return RepositorioObra.HorasLluviaEmpleadoEnObra(oe, fechaDesde, fechaHasta);
        }

        public List<ObraEmpleado> GetEmpleadosObra(Obra obra)
        {
            return RepositorioObra.GetEmpleadosObra(obra);
        }

        //public void AgregarEmpleadosAObra()
        //{
        //    RepositorioEmpleado.AgregarEmpleadosAObra();
        //}

        public bool AgregarEmpleadosAObraDTO(DateTime desde, DateTime hasta)
        {
            try
            {
                return RepositorioEmpleado.AgregarEmpleadosAObraDTO(desde, hasta);
            }
            catch(EmpleadoException e)
            {
                    throw new EmpleadoException(e.Message);
            }
            catch (AggregateException err)
            {
                throw new EmpleadoException(err.InnerException.Message);
            }
        }

        public void Precarga()
        {
            RepositorioEmpleado.Precarga();
            RepositorioPlano.Precarga();
        }

        public bool AgregarTodasLasMarcasDTO(DateTime desde, DateTime hasta)
        {
            try
            {
                return RepositorioEmpleado.ConseguirTodasLasMarcas(desde, hasta).Result;
            }
            catch (EmpleadoException e)
            {
                throw new EmpleadoException(e.Message);
            }
            catch (AggregateException err)
            {
                throw new EmpleadoException(err.InnerException.Message);
            }
        }

        public void AgregarTipoEmpleado(TipoEmpleado tipo)
        {
            RepositorioEmpleado.AgregarTipo(tipo);
        }

        public IEnumerable<TipoEmpleado> BuscarTiposEmpleados()
        {
            return RepositorioEmpleado.BuscarTipos();
        }

        public void AgregarEmpleado(Empleado empleado)
        {
            RepositorioEmpleado.Agregar(empleado);
        }

        public void DarEgreso(ObraEmpleado empleado, DateTime fecha)
        {
            RepositorioObra.DarEgreso(empleado, fecha);
        }

        public ObraEmpleado BuscarEmpleadoObra(int idEmpleado, int idObra)
        {
            return RepositorioObra.EmpleadoObra(idEmpleado, idObra);
        }
        
        //public void conseguirMarcasEmpleado(ObraEmpleado oe)
        //{
        //   RepositorioEmpleado.ConseguirMarcasDelEmpleado(oe);
        //}

        public void CambiarEstadoAVisto(Solicitud solicitud)
        {
            RepositorioSolicitud.CambiarEstadoAVisto(solicitud);
        }

        public List<Empleado> TomarEmpleadosDeObra(Obra obra)
        {
            return RepositorioEmpleado.TomarEmpleadosDeObra(obra);
        }

        public List<Marca> MarcasDelEmpleadoEnLaObra(List<Marca> marcas, Obra obra)
        {
            return RepositorioEmpleado.MarcasDelEmpleadoEnLaObra(marcas, obra);
        }

        public List<Marca> MarcasDelRangoDeFechas(List<Marca> marcas, DateTime desde, DateTime hasta)
        {
            return RepositorioEmpleado.MarcasDelRangoDeFecha(marcas, desde, hasta);
        }

        public List<Marca> TraerTodasMarcas(Empleado empleado)
        {
            return RepositorioEmpleado.TraerTodasMarcas(empleado);
        }

        public Dictionary<ObraEmpleado, double> Liquidar(DateTime desde, DateTime hasta, Obra? obra, Empleado? empleado)
        {
            return RepositorioEmpleado.Liquidar(desde, hasta, obra, empleado);
        }

        //public Dictionary<ObraEmpleado, double> LiquidacionTotal(DateTime desde, DateTime hasta)
        //{
        //    return RepositorioEmpleado.LiquidacionTotal(desde, hasta);
        //}

        //public Dictionary<ObraEmpleado, double> LiquidarEmpleadoObra(ObraEmpleado oe, DateTime desde, DateTime hasta)
        //{
        //    return RepositorioEmpleado.LiquidacionObraEmpleado(oe, desde, hasta);
        //}

        //public Dictionary<ObraEmpleado, double> LiquidarEmpleado(Empleado empleado, DateTime desde, DateTime hasta)
        //{
        //    return RepositorioEmpleado.LiquidacionEmpleado(empleado, desde, hasta);
        //}

        //public Dictionary<ObraEmpleado, double> LiquidarObra(Obra obra, DateTime desde, DateTime hasta)
        //{
        //    return RepositorioEmpleado.LiquidacionObra(obra, desde, hasta);
        //}

        public void AgregarTodasLasMarcasPorIdEmpleado(int idEmpleado)
        {
             RepositorioEmpleado.AgregarTodasLasMarcasPorIdEmpleado(idEmpleado);
        }

        public Obra BuscarObraPorNombre(string nombreObra)
        {
            return RepositorioObra.BuscarPorNombre(nombreObra);
        }

        public void ModificarTipo(TipoEmpleado tipo)
        {
            RepositorioEmpleado.ModificarTipo(tipo);
        }

        public IEnumerable<TipoEmpleado> TomarTodosTipoEmpleados()
        {
            return RepositorioEmpleado.BuscarTipos();
        }

        public TipoEmpleado BuscarTipo(int id)
        {
            return RepositorioEmpleado.BuscarTipo(id);
        }

        public int HorasTotales(List<Marca> marcas)
        {
            return RepositorioEmpleado.HorasTotales(marcas);
        }

        public bool MaterialesCheckStock(List<MaterialConsumoViewModel>? item, Obra obra)
        {
            return RepositorioObra.MaterialesCheckStock(item, obra);
        }

        public List<ObraMaterial> AlertarStockDeMaterialesTodasObras()
        {
            return RepositorioObra.AlertarStockDeMaterialesTodasObras();
        }

        public List<ObraMaterial> AlertarStockDeMaterialesTodasObrasACargo(string? nom)
        {
            return RepositorioObra.AlertarStockDeMaterialesTodasObrasACargo(nom);
        }

        public void ModificarObraEmpleado(ObraEmpleado obraEmpleadoNuevo)
        {
            RepositorioEmpleado.ModificarObraEmpleado(obraEmpleadoNuevo);
        }

        public double TotalLiquidacion(List<ObraEmpleadoLiquidacionViewModel> vm)
        {
            return RepositorioObra.CalcularTotalLiquidacion(vm);
        }

        public int TraerIdPorNombreObra(string name)
        {
            return RepositorioObra.TraerIdPorNombreObra(name);
        }

        public bool ExistePlano(byte[] plano, string planoNombre)
        {
            return RepositorioPlano.ExistePlano(plano, planoNombre);
        }
    }
}
