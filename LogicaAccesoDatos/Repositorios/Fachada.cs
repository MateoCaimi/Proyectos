using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public IEnumerable<Obra> BuscarPorNombre(string nombre)
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

        public void RechazarSolicitud(Solicitud solicitud,UDeOficina rechazador)
        {
            RepositorioSolicitud.RechazarSolicitud(solicitud,rechazador);
        }

        public List<Solicitud> BuscarSolicitudPendientesLista()
        {
            return RepositorioSolicitud.BuscarSolicitudPendientesLista();
        }

        public void ConfirmarSolicitud(Solicitud solicitud, Usuario logueado)
        {
            RepositorioSolicitud.ConfirmarSolicitud(solicitud, logueado);
        }

        public IEnumerable<ObraMaterial> MaterialesDeObra(int idObra)
        {
            return RepositorioObra.MaterialesDeObra(idObra);
        }

        public bool ConsumirMateriales(List<MaterialConsumoViewModel>? item, Obra obra)
        {
            return RepositorioObra.ConsumirMateriales(item, obra);
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

        public async Task<Task<string>> Liquidar()
        {
            return RepositorioEmpleado.Liquidar();
        }

        public void AsignacionHorasLluvia(Obra obra, int horasLluvia, DateTime dia)
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
            return GetEmpleadosObra(obra);
        }
    }
}
