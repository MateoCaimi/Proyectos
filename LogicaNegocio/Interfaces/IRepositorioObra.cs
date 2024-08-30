using LogicaNegocio.Entidades;

namespace LogicaNegocio.Interfaces
{

    public interface IRepositorioObra : IRepositorio<Obra>
    {
        LogicaNegocio.Entidades.Obra ObraPorNombre(System.String nombre);
        LogicaNegocio.Entidades.Obra ObraPorDireccion(System.String direccion);
        System.Collections.Generic.IEnumerable<LogicaNegocio.Entidades.Obra> BuscarPorDireccion(System.String direccion);
        LogicaNegocio.Entidades.Obra BuscarPorNombre(System.String nombre);
        void FinalizarObra(LogicaNegocio.Entidades.Obra obra);
        System.Collections.Generic.IEnumerable<LogicaNegocio.Entidades.Obra> ObrasFiltradas(System.String nombre, System.String direccion, System.Nullable<System.Boolean> finalizada);
        LogicaNegocio.Entidades.Material MaterialMasSolicitado(System.Int32 IdObra);
        LogicaNegocio.Entidades.Material MaterialMenosSolicitado(System.Int32 IdObra);
        LogicaNegocio.Entidades.Usuario SolicitanteMasComun(System.Int32 IdObra);
        LogicaNegocio.Entidades.UDeOficina AprobadorMasComun(System.Int32 IdObra);
        void AsignacionHorasLluvia(LogicaNegocio.Entidades.Obra obra, System.Int32 horas, System.DateTime dia);
        System.Collections.Generic.List<LogicaNegocio.Entidades.ObraEmpleado> GetEmpleadosObra(LogicaNegocio.Entidades.Obra obra);

    }
}
