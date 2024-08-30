using LogicaNegocio.Entidades;

namespace LogicaNegocio.Interfaces
{
    public interface IRepositorioEmpleado : IRepositorio<Empleado>
    {
        void AgregarEmp(LogicaNegocio.Entidades.Empleado item, System.Boolean desdeForm);
        System.Threading.Tasks.Task<System.String> LlamadaCloudtimes(System.DateTime desde, System.DateTime hasta);
        void Precarga();
        void PrecargaMarcasDelAño();
        System.Boolean AgregarEmpleadosAObraDTO(System.DateTime desde, System.DateTime hasta);
        void AgregarEmpleado(LogicaNegocio.Entidades.Empleado empleado, LogicaNegocio.Entidades.Obra obra);
        System.DateTime SetHoraA0(System.DateTime fecha);
        System.Threading.Tasks.Task<System.Boolean> ConseguirTodasLasMarcas(System.DateTime desde, System.DateTime hasta);
        System.Collections.Generic.IEnumerable<LogicaNegocio.Entidades.Obra> TomarTodasLasObras();
        System.Collections.Generic.Dictionary<LogicaNegocio.Entidades.ObraEmpleado, System.Decimal> Liquidar(System.DateTime desde, System.DateTime hasta, LogicaNegocio.Entidades.Obra obra, LogicaNegocio.Entidades.Empleado empleado, System.Boolean inactivos);
        System.Collections.Generic.Dictionary<LogicaNegocio.Entidades.ObraEmpleado, System.Decimal> LiquidacionEmpleado(LogicaNegocio.Entidades.Empleado empleado, System.DateTime desde, System.DateTime hasta, System.Boolean inactivos);
        System.Collections.Generic.Dictionary<LogicaNegocio.Entidades.ObraEmpleado, System.Decimal> LiquidacionObraEmpleado(LogicaNegocio.Entidades.ObraEmpleado oe, System.DateTime desde, System.DateTime hasta, System.Boolean inactivos);
        System.Collections.Generic.Dictionary<LogicaNegocio.Entidades.ObraEmpleado, System.Decimal> LiquidacionObra(LogicaNegocio.Entidades.Obra obra, System.DateTime desde, System.DateTime hasta, System.Boolean inactivos);
        System.Collections.Generic.Dictionary<LogicaNegocio.Entidades.ObraEmpleado, System.Decimal> LiquidacionTotal(System.DateTime desde, System.DateTime hasta, System.Boolean inactivos);
        void AgregarTipo(LogicaNegocio.Entidades.TipoEmpleado tipo);
        void ModificarTipo(LogicaNegocio.Entidades.TipoEmpleado tipo);
    }
}
