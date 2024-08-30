using LogicaNegocio.Entidades;
using LogicaNegocio.ViewModel;

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

        public IEnumerable<Obra> ObrasFiltradas(string? nombre, string? direccion, bool? finalizada)
        {
            return RepositorioObra.ObrasFiltradas(nombre, direccion, finalizada);
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

        public UDeOficina AprobadorMasComun(int IdObra)
        {
            return RepositorioObra.AprobadorMasComun(IdObra);
        }
        public void AgregarObra(Obra item)
        {
            RepositorioObra.Agregar(item);
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
        public IEnumerable<TipoPlano> BuscarTiposPlanos()
        {
            return RepositorioPlano.BuscarTiposPlanos();
        }
        public void AgregarPlano(Plano item)
        {
            RepositorioPlano.Agregar(item);
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

        public Solicitud CrearSolicitud(int idObra, int idSolicitante, string comentario)
        {
            return RepositorioSolicitud.CrearSolicitud(idObra, idSolicitante, comentario);
        }

        public void ConfigurarSolicitud(IEnumerable<SolicitudMaterial> laSolicitudConMateriales, Dictionary<int, int> materialesSeleccionadosConCantidad)
        {
            RepositorioSolicitud.ConfigurarMaterial(laSolicitudConMateriales, materialesSeleccionadosConCantidad);
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
        public void AsignacionHoras(Obra obra, int horasLluvia, DateTime dia)
        {
            RepositorioObra.AsignacionHorasLluvia(obra, horasLluvia, dia);
        }
        public List<ObraEmpleado> GetEmpleadosObra(Obra obra)
        {
            return RepositorioObra.GetEmpleadosObra(obra);
        }
        public void Precarga()
        {
            RepositorioEmpleado.Precarga();
            RepositorioPlano.Precarga();
        }
        public void AgregarTipoEmpleado(TipoEmpleado tipo)
        {
            RepositorioEmpleado.AgregarTipo(tipo);
        }

        public IEnumerable<TipoEmpleado> BuscarTiposEmpleados()
        {
            return RepositorioEmpleado.BuscarTipos();
        }

        public void AgregarEmpleado(Empleado empleado, bool desdeForm)
        {
            RepositorioEmpleado.AgregarEmp(empleado, desdeForm);
        }
        public ObraEmpleado BuscarEmpleadoObra(int idEmpleado, int idObra)
        {
            return RepositorioObra.EmpleadoObra(idEmpleado, idObra);
        }
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

        public Dictionary<ObraEmpleado, decimal> Liquidar(DateTime desde, DateTime hasta, Obra? obra, Empleado? empleado, bool inactivos)
        {
            return RepositorioEmpleado.Liquidar(desde, hasta, obra, empleado, inactivos);
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

        public decimal TotalLiquidacion(List<ObraEmpleadoLiquidacionViewModel> vm)
        {
            return RepositorioObra.CalcularTotalLiquidacion(vm);
        }

        public int TraerIdPorNombreObra(string name)
        {
            return RepositorioObra.TraerIdPorNombreObra(name);
        }
        public TipoPlano crearTipoPlanos(string nombreCarpeta, int idObra)
        {
            return RepositorioPlano.CrearTipoPlano(nombreCarpeta, idObra);
        }
        public List<TipoPlano> BuscarTiposPlanosPorObra(int idObra)
        {
            return RepositorioPlano.BuscarTipoPlanoPorObra(idObra);
        }
        public void CrearCarpeta(string path, string anterior, Obra obra)
        {
            RepositorioPlano.CrearCarpeta(path, anterior, obra);
        }
        public Carpeta ObtenerCarpeta(string name, Obra obra)
        {
            return RepositorioPlano.BuscarCarpeta(name, obra);
        }

        public Carpeta ObtenerRoot(Obra obra)
        {
            return RepositorioPlano.BuscarRoot(obra);
        }

        public void MapearTíposACarpetas()
        {
            RepositorioPlano.MapearTiposACarpetas();
        }

        public List<Carpeta> CarpetasPosteriores(Carpeta carpeta)
        {
            return RepositorioPlano.CarpetasPosteriores(carpeta);
        }

        public void LimpiarCarpetasYTipos(int idObra)
        {
            RepositorioPlano.LimpiarCarpetasYTipos(idObra);
        }

        public bool ActualizarPlanosEnObra(Obra obra)
        {
            return RepositorioObra.ActualizarPlanosEnObra(obra);
        }

        public void ActualizarFechaUltimaActualizacion(Obra obra)
        {
            RepositorioObra.ActualizarFechaUltimaActualizacion(obra);
        }

        public void PrecargaMarcas()
        {
            RepositorioEmpleado.PrecargaMarcasDelAño();
        }

        public List<Solicitud> BuscarSolicitudAprobadasPorObra(string? nomObrero)
        {
            return RepositorioSolicitud.BuscarSolicitudesAprobadasPorObra(nomObrero);
        }

        internal Obra BuscarObraPorCapataz(string? nomObrero)
        {
            return RepositorioObra.BuscarObraPorCapataz(nomObrero);
        }
    }
}
