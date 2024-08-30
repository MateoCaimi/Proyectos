using LogicaNegocio.Entidades;

namespace LogicaNegocio.Interfaces
{
    public interface IRepositorioPlano : IRepositorio<Plano>
    {
        void Precarga();
        System.Collections.Generic.IEnumerable<LogicaNegocio.Entidades.Plano> BuscarPlanosDelTipoEnObra(System.Int32 idTipoPlano, System.Int32 idObra);
        System.Collections.Generic.IEnumerable<LogicaNegocio.Entidades.Plano> PlanosFiltrados(LogicaNegocio.Entidades.Obra obra, System.Nullable<System.Int32> idTipo, System.String nombre, System.Nullable<System.DateTime> fechaDesde, System.Nullable<System.DateTime> fechaHasta);
        System.Collections.Generic.IEnumerable<LogicaNegocio.Entidades.Plano> PlanosPorAntiguedad(LogicaNegocio.Entidades.Obra obra);
        System.Collections.Generic.IEnumerable<LogicaNegocio.Entidades.Plano> PlanosTotales(LogicaNegocio.Entidades.Obra obra);
        System.Collections.Generic.IEnumerable<LogicaNegocio.Entidades.TipoPlano> BuscarTiposPlanos();
        LogicaNegocio.Entidades.TipoPlano BuscarTipoPlano(System.Int32 idTipo);
        LogicaNegocio.Entidades.TipoPlano CrearTipoPlano(System.String nombreCarpeta, System.Int32 idObra);
        void EliminarTipoPlano(LogicaNegocio.Entidades.TipoPlano tipoPlanoActual);
        System.Collections.Generic.List<LogicaNegocio.Entidades.TipoPlano> BuscarTipoPlanoPorObra(System.Int32 idObra);
        void CrearCarpeta(System.String path, System.String anterior, LogicaNegocio.Entidades.Obra obra);
        LogicaNegocio.Entidades.TipoPlano BuscarTipoPlanoPorNombre(System.String name, LogicaNegocio.Entidades.Obra obra);
        LogicaNegocio.Entidades.Carpeta BuscarCarpeta(System.String path, LogicaNegocio.Entidades.Obra obra);
        void MapearTiposACarpetas();

    }
}
